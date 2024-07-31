using System.Globalization;
using System.Linq;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public struct CoffeeOptions
{
    public int Shots;
    public int Syrups;
    public int WaterLevel;
}

public class CoffeeOrder : MonoBehaviour
{
    [SerializeField] private Coffee coffeePrefab;
    [SerializeField] private Transform coffeeHolder;
    [SerializeField] private TMP_Text shotText; 
    [SerializeField] private TMP_Text syrupText;
    [SerializeField] private TMP_Text waterLevelText;
    [SerializeField] private TMP_Text costText;

    private Coffee coffee;
    private ICoffee coffeeOrder;
    private CoffeeOptions options;
    private string[] waterLevelLabels = { "low", "medium", "high" };
    
    private void Start()
    {
        coffee = Instantiate(coffeePrefab, coffeeHolder);
        coffeeOrder = new EspressoShotDecorator(coffee);
        coffeeOrder.Add();
        coffeeOrder = new WaterLevelDecorator(coffeeOrder);
        coffeeOrder.Add();

        options = new CoffeeOptions
        {
            Shots = 1,
            WaterLevel = 1
        };
        shotText.text = options.Shots.ToString();
        syrupText.text = options.Syrups.ToString();
        waterLevelText.text = waterLevelLabels[options.WaterLevel - 1];
        costText.text = coffeeOrder.Cost.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeShots(bool increase)
    {
        if ((increase && options.Shots == 10) || (!increase && options.Shots == 1)) return;
        
        coffeeOrder = new EspressoShotDecorator(coffeeOrder);
        if (increase)
        {
            options.Shots++;
            coffeeOrder.Add();
        }
        else
        {
            options.Shots--;
            coffeeOrder.Remove();
        }
        shotText.text = options.Shots.ToString();
        costText.text = coffeeOrder.Cost.ToString(CultureInfo.InvariantCulture);
    }

    public void ChangeSyrup(bool increase)
    {
        if ((increase && options.Syrups == 10) || (!increase && options.Syrups == 0)) return;
        
        coffeeOrder = new SyrupDecorator(coffeeOrder);
        if (increase)
        {
            options.Syrups++;
            coffeeOrder.Add();
        }
        else
        {
            options.Syrups--;
            coffeeOrder.Remove();
        }
        syrupText.text = options.Syrups.ToString();
        costText.text = coffeeOrder.Cost.ToString(CultureInfo.InvariantCulture);
    }
    
    public void ChangeWaterLevel(bool increase)
    {
        if ((increase && options.WaterLevel == 3) || (!increase && options.WaterLevel == 1)) return;
        
        coffeeOrder = new WaterLevelDecorator(coffeeOrder);
        if (increase)
        {
            options.WaterLevel++;
            coffeeOrder.Add();
        }
        else
        {
            options.WaterLevel--;
            coffeeOrder.Remove();
        }
        waterLevelText.text = waterLevelLabels[options.WaterLevel - 1];
    }
    
    public void OrderCoffee()
    {
        coffee.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        coffee.Brew(coffeeOrder.Ingredients.Values.ToList());
        Debug.Log($"Ingredients: {string.Join(", ", coffee.Ingredients.Select(i => $"{i.Key}: {i.Value.Count}"))}");
    }
}
