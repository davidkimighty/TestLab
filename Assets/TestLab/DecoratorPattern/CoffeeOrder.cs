using System.Linq;
using TMPro;
using UnityEngine;

public struct CoffeeOptions
{
    public int Shots;
    public int Syrups;
}

public class CoffeeOrder : MonoBehaviour
{
    [SerializeField] private Coffee coffeePrefab;
    [SerializeField] private Transform coffeeHolder;
    [SerializeField] private TMP_Text shotText; 
    [SerializeField] private TMP_Text syrupText;

    private ICoffee coffee;
    private CoffeeOptions options;
    
    private void Start()
    {
        coffee = Instantiate(coffeePrefab, coffeeHolder);
        coffee = new EspressoShotDecorator(coffee);
        coffee.Add();
        options.Shots = 1;
        
        shotText.text = options.Shots.ToString();
        syrupText.text = options.Syrups.ToString();
    }

    public void ChangeShots(bool increase)
    {
        coffee = new EspressoShotDecorator(coffee);
        if (increase)
        {
            if (options.Shots == 5) return;
            options.Shots++;
            coffee.Add();
        }
        else
        {
            if (options.Shots == 1) return;
            options.Shots--;
            coffee.Remove();
        }
        shotText.text = options.Shots.ToString();
        Debug.Log($"Ingredients: {string.Join(", ", coffee.Ingredients.Select(i => $"{i.Key}: {i.Value}"))}" +
                  $"\nCost: {coffee.Cost}");
    }

    public void ChangeSyrup(bool increase)
    {
        coffee = new SyrupDecorator(coffee);
        if (increase)
        {
            if (options.Syrups == 5) return;
            options.Syrups++;
            coffee.Add();
        }
        else
        {
            if (options.Syrups == 0) return;
            options.Syrups--;
            coffee.Remove();
        }
        syrupText.text = options.Syrups.ToString();
        Debug.Log($"Ingredients: {string.Join(", ", coffee.Ingredients.Select(i => $"{i.Key}: {i.Value}"))}" +
                  $"\nCost: {coffee.Cost}");
    }
    
    public void OrderCoffee()
    {
        
    }
    
    
}
