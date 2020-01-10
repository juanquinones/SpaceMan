using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollectableType //creamos un enumerado para configurar todos los colleccionables
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;  //variable de collectable de tipo money para configurar la moneda

    private SpriteRenderer sprite; //accedemos a su sprite
    private CircleCollider2D itemCollider; //accedemos a su collider 

    bool hasBeenCollected = false; //variable que nos indica si ya fue o no colleccionada

    public int value = 1; //valor del collecionable

    GameObject player; //variable que llama al objeto player 

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>(); //llamamos el componente sprite del objeto al igual que su collider
        itemCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = GameObject.Find("Player"); //inicializamos el objeto player buscandolo por su tag name
    }

    void Show() //metodo para mostrar la moneda
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide() //metodo para esconder la moneda
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect() //metodo para collecionar la moneda
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type) //utilizamos un switch para dependiendo del collecionable programar su acción
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this); //se llama al metodode CollectObject en el game manager para incrementar el contador en el valor de moneda
                GetComponent<AudioSource>().Play(); //activamos el sonido de recolectar moneda
                break;

            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value); //buscamos el componente PlayerController del objeto player y especificamente el metodo health
                break;

            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
        }
    }
     
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Player")
        {
            Collect(); //si el jugador colisiona contra un colleccionable entonces se llama al metodo collect
        }
    }
}
