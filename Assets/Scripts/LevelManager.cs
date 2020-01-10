using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour { 

    
    public static LevelManager sharedInstance; //crear un singleton para el controlador de nivel

    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>(); // se crea una lista para contener todos los bloques de nivel prefabricados

    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>(); //lista que contendra todos los bloques de la escena

    public Transform levelStartPosition; //variable que guarda la posición inicial del nivel 

    void Awake() //configurar el singleton en el awake para que solo sea ese.
    {
        if (sharedInstance == null)
        {
            sharedInstance = this; 
        }
    }  

// Start is called before the first frame update
    void Start()
    {
        GenerateInicialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLevelBlock() //añadir un bloque mientras el personaje avanza en la partida
    {
        int randomIdx = Random.Range(1, allTheLevelBlocks.Count); // generar un numero aleatorio uniforme por el RANGE osea que son equiprobables los resultados

        LevelBlock block; //obtener un bloque

        Vector3 spawnPosition = Vector3.zero; //posición donde se colocara el bloque

        if(currentLevelBlocks.Count == 0) // si el numero de bloques actuales es 0
        {
            block = Instantiate(allTheLevelBlocks[0]); //que el primer bloque de nivel sea el levelblock 0
            spawnPosition = levelStartPosition.position; //otorgarle la posisión inicial del nivel
        }
        else
        {
            block = Instantiate(allTheLevelBlocks[randomIdx]); //montar un bloque de nivel aleatorio
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position; //que la posicion del bloquea sea el final del bloque anterior
        }

        block.transform.SetParent(this.transform,false); //hacer que los bloques sean hijos de el level manager

        Vector3 correction = new Vector3(
                spawnPosition.x-block.startPoint.position.x,
                spawnPosition.y-block.startPoint.position.y,
                0);
        block.transform.position = correction;
        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock() //remover bloques cuando ya el personaje pase por ellos
    {
        LevelBlock oldBlock = currentLevelBlocks[0]; //el bloque viejo que queremos destruir
        currentLevelBlocks.Remove(oldBlock); //invocamos el metodo remove para eliminar
        Destroy(oldBlock.gameObject); //y al eliminarlos los destruimos
    }

    public void RemoveAllLevelBlocks() //eliminar todos lops bloques al morir el personaje
    {
        while(currentLevelBlocks.Count > 0) //mientras la cuenta de bloques actuales sea mayor a 0 eliminarlos.
        {
            RemoveLevelBlock();
        }
    }

    public void GenerateInicialBlocks() //Añadir bloques iniciales
    {
        for (int i = 0; i < 10; i++) //ciclo que ejecuta el metodo addlevelblock 
        {
            AddLevelBlock();
        }
    }


}
