using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotions : MonoBehaviour
{

    private Image[] healthFlasks;
    [SerializeField] private Image emptyFlask;
    private Image fillFlask;

    // Start is called before the first frame update
    void Start()
    {
        healthFlasks = GetComponentsInChildren<Image>();
        fillFlask = healthFlasks[0];

    }

    // Update is called once per frame

    public void SetPotionsFill(int numberOfEmptyPotion)
    {
        healthFlasks[numberOfEmptyPotion].sprite = fillFlask.sprite;
    }
    
    public void SetPotionsEmpty(int numberOfFillPotion)
    {
        healthFlasks[numberOfFillPotion].sprite = emptyFlask.sprite;
        
    }
}
