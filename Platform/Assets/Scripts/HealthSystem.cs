using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;

    [SerializeField] private HealthPotions healthPotions;
    //vita del player
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private int healing;
    private int flasks;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetHealthBarMaxValue(maxHealth);
        currentHealth = maxHealth;
        healing = 20;
        flasks = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (flasks <3 && currentHealth<100)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Healing();
                flasks++;

            }
        }
      
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(10);

        }

    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        healthBar.SetHealthBar(currentHealth-damage);
        currentHealth -= damage;
    }

    public void RestoreHealthAndPotions()
    {
        healthBar.SetHealthBar(maxHealth);
        healthPotions.SetPotionsFill(0);
        healthPotions.SetPotionsFill(1);
        healthPotions.SetPotionsFill(2);
        
    }

    public void Healing()
    {
        Debug.Log("cura");
    
        if (currentHealth + healing>100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += healing;
          
        }
        
        healthBar.SetHealthBar(currentHealth);
        healthPotions.SetPotionsEmpty(flasks);
        



    }
}
