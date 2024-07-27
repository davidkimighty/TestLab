using TMPro;
using UnityEngine;

public struct CoffeeOptions
{
    public int Shots;
    public int Syrups;
    
}

public class CoffeeOrder : MonoBehaviour
{
    [SerializeField] private TMP_Text shotText; 
    [SerializeField] private TMP_Text syrupText;
    [SerializeField] private Coffee coffee;
    
    private CoffeeOptions options = new();
    
    private void Start()
    {
        options.Shots = 1;
        shotText.text = options.Shots.ToString();
        syrupText.text = options.Syrups.ToString();
    }

    public void ChangeShots(bool increase)
    {
        if (increase)
        {
            if (options.Shots == 5) return;
            options.Shots++;
        }
        else
        {
            if (options.Shots == 1) return;
            options.Shots--;
        }
        shotText.text = options.Shots.ToString();
    }

    public void ChangeSyrup(bool increase)
    {
        if (increase)
        {
            if (options.Syrups == 5) return;
            options.Syrups++;
        }
        else
        {
            if (options.Syrups == 0) return;
            options.Syrups--;
        }
        syrupText.text = options.Syrups.ToString();
    }
    
    public void OrderCoffee()
    {
        
    }
}
