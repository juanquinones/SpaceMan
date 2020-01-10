using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 2f; //velocidad de movimiento del enemigo

    Rigidbody2D rigidBody; //variable para cargar el componente rigidbody del enemigo

    public bool facingRight = false; //variable booleana que nos dira si el enemigo esta mirando hacia la derecha o no
    public int enemyDamage = 10; // valor de daño del enemigo hacia el jugador
    private Vector3 startPosition; //variable que alamacenara la posición del enemigo

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); //cargamos el componente rigidbody en el awake
        startPosition = this.transform.position; //asi sabremos donde arranco el personaje
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() //update que va a intervalos fijos para no experimentar bajadas de frame
    {
        float currentRunningSpeed = runningSpeed;

        if (facingRight) //calcular la velocidad y dirección del enemigo
        {
            //mirando hacia la derecha
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);//permite rotar a 180 grados la posición del personaje tomando en cuenta que mira a la izquierda por defecto
        }
        else
        {
            //mirando hacia la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero; //lo devuelve a su posición original
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) // si esta en la partida
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y); //se movera en el eje de las x, mientras el eje de las y mantendra su direccion actual.
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin") //si colisiona contra una moneda no hacer nada
        {
            return;
        }

        if(collision.tag == "Player") // si colisiona con un jugador llamar al metodo CollectHealth y pasarle un parametro negativo para que baje la vida
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            GetComponent<AudioSource>().Play(); //sonido al hacer daño
            return;
        }

        //si no chocamos contra una moneda o un jugador
        //es que chocamos contra otro enemigo o escenario
        // entonces rotaremos al enemigo

        facingRight = !facingRight;
    }
}
