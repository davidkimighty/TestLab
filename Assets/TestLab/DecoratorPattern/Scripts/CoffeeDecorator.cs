using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CoffeeDecorator : ICoffee
{
    protected ICoffee coffee;
        
    public decimal Cost { get; set; }
    public Dictionary<string, BrewOperation> Ingredients { get; set; } = new();

    protected CoffeeDecorator(ICoffee coffee)
    {
        this.coffee = coffee;
    }
        
    public virtual void Add()
    {
        foreach (string key in Ingredients.Keys)
        {
            if (!coffee.Ingredients.TryAdd(key, new BrewOperation(1, Brew)))
                coffee.Ingredients[key].Count += 1;
        }
        Ingredients = coffee.Ingredients;
        Cost += coffee.Cost;
    }

    public virtual void Remove()
    {
        foreach (string key in Ingredients.Keys.Intersect(coffee.Ingredients.Keys).ToList())
        {
            if (!coffee.Ingredients.TryGetValue(key, out BrewOperation brew) || brew.Count <= 0) continue;
            coffee.Ingredients[key].Count -= 1;
        }
        Ingredients = coffee.Ingredients;
        Cost = coffee.Cost - Cost;
    }

    public abstract void Brew(Liquid liquid);
}

public class EspressoShotDecorator : CoffeeDecorator
{
    public EspressoShotDecorator(ICoffee coffee) : base(coffee)
    {
        Cost = .3M;
        Ingredients.Add("Espresso", new BrewOperation(1, Brew));
    }

    public override void Brew(Liquid liquid)
    {
        liquid.ReduceColorIntensity(1f, Liquid.TopColorId);
        liquid.ReduceColorIntensity(1f, Liquid.BottomColorId);
        liquid.ReduceColorIntensity(1f, Liquid.FoamColorId);
    }
}

public class SyrupDecorator : CoffeeDecorator
{
    public SyrupDecorator(ICoffee coffee) : base(coffee)
    {
        Cost = .5M;
        Ingredients.Add("Hazelnut Syrup", new BrewOperation(1, Brew));
    }
    
    public override void Brew(Liquid liquid)
    {
        Color syrupColor = new Color32(185, 107, 92, 255); 
        liquid.MixColor(syrupColor, 0.01f, Liquid.TopColorId);
        liquid.MixColor(syrupColor, 0.01f, Liquid.BottomColorId);
        liquid.MixColor(syrupColor, 0.01f, Liquid.FoamColorId);
    }
}

public class WaterLevelDecorator : CoffeeDecorator
{
    public WaterLevelDecorator(ICoffee coffee) : base(coffee)
    {
        Cost = 0;
        Ingredients.Add("Water", new BrewOperation(1, Brew));
    }
    
    public override void Brew(Liquid liquid)
    {
        liquid.AddFillAmount(-0.11f);
    }
}