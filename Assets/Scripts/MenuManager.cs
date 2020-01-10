using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager sharedInstance;
    public Canvas menuCanvas; //variable que llama al objeto canvas
    public Canvas gameOverCanvas;//variable que llama al canvas de game over
    public Canvas inGameCanvas;// variable que llama al canvas del juego

    void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true; // activar el canvas
      
    }

    public void HideMainMenu()
    {
        menuCanvas.enabled = false; // desactivar canvas
    }

    public void ShowGameMenu()
    {
        inGameCanvas.enabled = true; // activar el canvas
    }

    public void HideGameMenu()
    {
        inGameCanvas.enabled = false; // desactivar canvas
    }

    public void ShowGameOverMenu()
    {
        gameOverCanvas.enabled = true; // activar el canvas
    }

    public void HideGameOverMenu()
    {
        gameOverCanvas.enabled = false; // desactivar canvas
    }


    public void ExitGame()
    {
       #if UNITY_EDITOR   //se llama al editor de unity para poder hacer el cierre desde su plataforma            
       UnityEditor.EditorApplication.isPlaying = false; //forzamos la salida del juego
       #else
       Application.Quit(); //de lo contrario si no estamos en unity se cierra la aplicacion.
       #endif 
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
