using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;

    public void SetHealthBar(int currentHealth)
    {
        slider.value = currentHealth;
      
    }
    
    public void SetHealthBarMaxValue(int health)
    {
        slider.maxValue = health;
        slider.value = health;

    }


}
