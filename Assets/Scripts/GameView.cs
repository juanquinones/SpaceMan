using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    private PlayerController controller; //llamamos al controlador del jugador para luego acceder a los metodos que necesitamos.

    public Text coinsText, scoreText, maxScoreText; //llamamos los textos del canvas a manipular
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>(); //llamamos a la variable controller el objeto player y llamamos su componente 
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame) //llamar al estado in game para que se ejecuten los contadores al iniciar el juego
        {
            int coins = GameManager.sharedInstance.collectedObject; //crear variables para los contadores
            float score = controller.GetTravelledDistance(); // el score del jugador sera igual al metodo travelledDistance que hemos configurado
            float maxScore = PlayerPrefs.GetFloat("maxscore", 0f);

            coinsText.text = coins.ToString(); //convertir las variables numericas a texto y pasarsela a la variable tipo texto del canvas
            scoreText.text = "Score: " + score.ToString("f1"); //se le asigna el parametro f1 para quedarnos con un solo decimal
            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
        }
    }
}
