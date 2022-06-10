using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameMaster gm;
    
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("checkpoint");
        if (other.CompareTag("Player"))
        { 
            Debug.Log(other);
          other.GetComponent<HealthSystem>().RestoreHealthAndPotions();
            gm.lastCheckPointPos = transform.position;
        }
    }

  
}
