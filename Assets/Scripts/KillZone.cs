using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //se ejecuta un metodo cuando un collider entra dentro de otro
    {
        if (collision.tag == "Player") //si la etiqueta de la collision es Player se ejecuta
        {
            PlayerController controller = collision.GetComponent<PlayerController>(); // permite recuperar el controlador del personaje
            controller.Die(); //se llama al metodo Die
        }
    }
}
