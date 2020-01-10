using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //variable que me permitira buscar el objetivo de la camara

    public Vector3 offset = new Vector3(0.2f, 0.0f, -10f); //ajusta la distancia a la que estara la camara del objetivo

    public float dampingTime = 0.3f; //tiempo de amortiguación para que la camara se mueva con el personaje

    public Vector3 velocity = Vector3.zero; // variable de velocidad de camara

    void Awake()
    {
        Application.targetFrameRate = 60; //determinar que la camara vaya a 60 frames por segundo. 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true); //queremos hacer movimiento suavizado mientras se renderiza
    }

    public void ResetCameraPosition() //metodo para recetear la camara al morir el personaje
    {
        MoveCamera(false); // no necesitamos hacer movimiento barrido
    }

    void MoveCamera(bool smooth) // servira para hacer un movimiento suavizado de camara
    {
       Vector3 destination = new Vector3( // variable que sigue el objetivo de camara
            target.position.x - offset.x, //sigue al objetivo en x pero un poco desplazado
            target.position.y - offset.y, 
            offset.z); 

        if (smooth) //hacer el barrido suavizado si el parametro esta activo.
        {
            this.transform.position = Vector3.SmoothDamp( //este vector tiene 4 parametros
                this.transform.position, //posicion actual de la camara
                destination, //objetivo al que se quiere ir
                ref velocity, //paso por referencia de velocidad
                dampingTime); // tiempo de animacion de camara

        } else //de lo contrario solo se movera la camara sin nigun efecto
        {
            this.transform.position = destination; 
        }


    } 


}
