﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //metodo que destruira los bloques de nivel al personaje pasar por el 
    {
        if(collision.tag == "Player")
        {
            LevelManager.sharedInstance.AddLevelBlock();
            LevelManager.sharedInstance.RemoveLevelBlock();
        }
        
    }
}
