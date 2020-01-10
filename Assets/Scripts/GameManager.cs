using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState //enumeracion de estados del juego
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu; // estado del juego empezara siendo en el menu

    public static GameManager sharedInstance; //variable para compartir instancias

    private PlayerController controller; // variable para llamar al player controller

    public int collectedObject = 0; //variable que contara cuantos objetos se agarran

    private void Awake()
    {
        if (sharedInstance == null) // si el sharedInstance no tiene valor conocido, queda asignado a this (este mismo script) 
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame) //al oprimir tecla enter pasar a la funcion StartGame
        {
            StartGame();
        }
    }

    public void StartGame() //Metodo encargado de iniciar la partida
    {
        SetGameState(GameState.inGame);

    }

    public void GameOver() //Metodo encargado de finalizar la partida
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu() //metodo que me retornara al menu del juego
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState) //metodo que cambia el estado del juego
    {
        if (newGameState == GameState.menu)
        {
            // TODO: colocar la lógica del menú
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.ShowMainMenu(); //se activa el menu

        } else if(newGameState == GameState.inGame)
        {
            // TODO: hay que preparar la escena para jugar
            LevelManager.sharedInstance.RemoveAllLevelBlocks(); // remover todos los bloques antes de cargar de nuevo la partida                                                      
            LevelManager.sharedInstance.GenerateInicialBlocks(); //se crean los bloques de inicio de partida.  
            controller.StartGame();

            MenuManager.sharedInstance.ShowGameMenu();//mostrar el canvas del juego
            MenuManager.sharedInstance.HideMainMenu(); //se esconde el menu al iniciar la partida
            MenuManager.sharedInstance.HideGameOverMenu();

        } else if(newGameState == GameState.gameOver)
        {
            // TODO: preparar el juego para el Game Over
            MenuManager.sharedInstance.ShowGameOverMenu();//se activa el menu de game over
            MenuManager.sharedInstance.HideMainMenu(); 
            MenuManager.sharedInstance.HideGameMenu();

        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable) //metodo que recolecta un valor collectable y lo incrementa en el contador
    {
        collectedObject += collectable.value; // lo mismo que decir collectedObject = collectedObject + collectable.value;
    }
}
