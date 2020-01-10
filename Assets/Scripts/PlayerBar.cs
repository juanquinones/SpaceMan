using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType //enumerado para los dos tipos de barras que tendremos
{
    healthBar,
    manaBar
}

public class PlayerBar : MonoBehaviour
{
    private Slider slider; //variable slider para llamar al objeto slider de la barra
    public BarType type; //llamamos a la variable enum publica para verla en el unity
    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>(); //llamamos al componente slider

        switch (type) //creamos un switch para ajustar los valores de las barras para cada uno de sus casos
        {
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH; //llamammos el método GetHealth para que nos de el valor maximo de vida del personaje 
                break;

            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA; //llamammos el método GetMana para que nos de el valor maximo  de mana del personaje 
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (type) //switch que actualiza los valores de las barras
        {
            case BarType.healthBar:
                slider.value = GameObject.Find("Player").
                    GetComponent<PlayerController>().GetHealth(); //llamammos el método GetHealth para que nos de el valor actual de vida del personaje 
                break;

            case BarType.manaBar:
                slider.value = GameObject.Find("Player").
                    GetComponent<PlayerController>().GetMana(); //llamammos el método GetMana para que nos de el valor actual  de mana del personaje 
                break;
        }
    }  
}
