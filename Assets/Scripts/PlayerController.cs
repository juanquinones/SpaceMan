using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del movimiento del Personaje
    public float jumpForce = 6f;
    public float runningSpeed = 2f;

    Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition; //variable que guarda el vector inicial del personaje

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";

    [SerializeField] //para ver las variables privadas en el gestor de unity
    private int healthPoints, manaPoints; //variables que trackean el estado actual de vida y mana del jugador.

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, //variables que delimitan los rangos de la vida y el mana del jugador
        MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 0, MIN_MANA = 0;

    public LayerMask groundMask; //Capa que se encarga de conocer quien es parte del suelo

    public const int SUPERJUMP_COST = 5;  //coste del super salto en mana
    public const float SUPERJUMP_FORCE = 1.5f; // fuerza der super salto

    public float jumpRaycastDistance = 1.7f; //variable de longitud del raycast

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); // se manda a llamar la variable de rigidbody para su uso posteriormente
        animator = GetComponent<Animator>(); 
    }

    // Start is called before the first frame update
    void Start()
    {

        startPosition = this.transform.position; //la posicion del personaje sera igual a su posición a la hora del start
    }

    public void StartGame() //metodo que inicia el juego 
    { 
        animator.SetBool(STATE_ALIVE, true); // declarar el estado de inicio del parametro del animator
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH; //iniciamos las variables de vida y de mana con su valores iniciales.
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.3f); //Método que invoca un metodo y permite retrasar su ejecución
    }

    void RestartPosition() //metodo que restaura la posición del jugador al volver a iniciar partida
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero; // se realentiza la velocidad del personaje para liberarnos de un bug cuando caiga.

        GameObject mainCamera = GameObject.Find("Main Camera"); //llamamos a la camara 
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition(); //llamamos a la funcion resetCameraPosition
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetButtonDown("Jump")) //condicional para que se ejecute metodo jump al oprimir espacio o click derecho
            {
                Jump(false);
            }

            if (Input.GetButtonDown("SuperJump")) //condicional que permite ejecutar el metodo jump como superjump
            {
                Jump(true); //le pasamos un paramerto true para confirmar que es un salto de tipo superjump
            }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround()); //permite que la el valor de la constante se actualice y sea igual al valor del parametro IsTouchingTheGround.

        Debug.DrawRay(this.transform.position, Vector2.down * jumpRaycastDistance, Color.red); //utilizamos Gizmos para visualizar el Raycast que implementamos
    }

    private void FixedUpdate() // un update que funciona a ritmo fijo, que no se retraza y no acelera 
    {

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) //permite que el pesonaje se mueva solo si esta en estado de juego
        {
            if (Input.GetKey(KeyCode.RightArrow)) //Movimiento del personaje
            {
                rigidBody.velocity = new Vector2(runningSpeed, // eje x
                                                 rigidBody.velocity.y // eje y
                                                 );
                transform.localScale = new Vector2(1f, 1f); //agarra la escala del transform del personaje y la modifica con un nuevo vector para la animación
            }

            else if (Input.GetKey(KeyCode.LeftArrow)) //Movimineto izquierda
            {
                rigidBody.velocity = new Vector2(runningSpeed * -1, rigidBody.velocity.y);

                transform.localScale = new Vector2(-1f, 1f);

            }

            else //dejar de caminar para no deslizar
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }


        } else //si no estamos dentro de la partida
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

    }

    void Jump(bool superjump) //con el parametro booleano cotejamos si es un salto normal o un superjump
    {
        float jumpForceFactor = jumpForce; // variable local que llamamos para poder unir el valor del jumpforce

        if (superjump && manaPoints >= SUPERJUMP_COST) //condicional que activara el super salto si hay mana suficiente y le restara mana al usarlo
        {
            manaPoints -= SUPERJUMP_COST; //decrementamos el numero de mana que tenemos
            jumpForceFactor *= SUPERJUMP_FORCE; //multiplicamos la fuerza de salto
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) // solo puede saltar si esta en modo juego
        {
            if (IsTouchingTheGround()) // si toca el suelo puede saltar
            {
                GetComponent<AudioSource>().Play();//activamos el sonido al saltar
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }
        }
    }

    bool IsTouchingTheGround() //permite usar el raycast y verificque si el peronaje esta o no en el suelo
    {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, jumpRaycastDistance, groundMask))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void Die() //metodo de muerte, se hace publico para invocarlo desde KillZone
    {
        float travelledDistance = GetTravelledDistance(); //llamamamos al metodo GetTravelledDistance
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f); //para crear una variable que nos guarde la puntuación maxima y le asignamos un valor de 0 si no existe un score todavia
        if(travelledDistance > previousMaxDistance) //condicional que guarda el puntaje en el maxscore si el score llega a superarlo
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
            

        this.animator.SetBool(STATE_ALIVE, false); //se cambia el estado del personaje a muerto
        GameManager.sharedInstance.GameOver(); // se cambia el estado del juego a gameover
    }

    public void CollectHealth(int points) //metodo que incrementa vida
    {
        this.healthPoints += points; //incrementamos la variable healthPoints con los puntos que llegan por parametro
        if (this.healthPoints >= MAX_HEALTH) //si los puntos de vida superan la vida maxima se iguala a la maxima vida para que no la supere
        {
            this.healthPoints = MAX_HEALTH;
        }
        if (this.healthPoints <=0)
        {
            Die();
        }
    }

    public void CollectMana(int points) //metodo que incrementa mana
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth() //metodo que devuelve el valor de vida
    {
        return healthPoints;
    }

    public int GetMana() //metodo que devuelve el valor de mana
    {
        return manaPoints;
    }

    public float GetTravelledDistance() //metodo que calcula la distancia en la que estoy menos la distancia del punto de inicio para calcular la distancia total recorrida
    {
        return this.transform.position.x - startPosition.x;
    }
}
