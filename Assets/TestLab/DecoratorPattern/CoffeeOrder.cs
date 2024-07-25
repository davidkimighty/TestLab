using System;
using TMPro;
using UnityEngine;

public class CoffeeOrder : MonoBehaviour
{
    [SerializeField] private TMP_Text shotText; 
    [SerializeField] private TMP_Text syrupText; 
    [SerializeField] private Liquid liquid;

    private int shots = 1;
    private int syrups = 0;
    
    private void Start()
    {
        shotText.text = shots.ToString();
        syrupText.text = syrups.ToString();
    }

    public void ChangeShots(bool increase)
    {
        if (increase)
        {
            if (shots == 5) return;
            shots++;
        }
        else
        {
            if (shots == 1) return;
            shots--;
        }
        shotText.text = shots.ToString();
    }

    public void ChangeSyrup(bool increase)
    {
        if (increase)
        {
            if (syrups == 5) return;
            syrups++;
        }
        else
        {
            if (syrups == 0) return;
            syrups--;
        }
        syrupText.text = syrups.ToString();
    }
    
    public void OrderCoffee()
    {
        
    }
}
