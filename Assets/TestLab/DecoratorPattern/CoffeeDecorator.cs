using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CoffeeDecorator : ICoffee
{
    protected ICoffee coffee;
        
    public decimal Cost { get; set; }
    public Dictionary<string, int> Ingredients { get; set; } = new();

    protected CoffeeDecorator(ICoffee coffee)
    {
        this.coffee = coffee;
    }
        
    public virtual void Add()
    {
        foreach (string key in Ingredients.Keys)
        {
            if (!coffee.Ingredients.TryAdd(key, 1))
                coffee.Ingredients[key] += 1;
        }
        Ingredients = coffee.Ingredients;
        Cost += coffee.Cost;
    }

    public virtual void Remove()
    {
        foreach (string key in Ingredients.Keys.Intersect(coffee.Ingredients.Keys).ToList())
        {
            if (!coffee.Ingredients.TryGetValue(key, out int count) || count <= 0) continue;
            coffee.Ingredients[key] -= 1;
        }
        Ingredients = coffee.Ingredients;
        coffee.Cost -= Cost;
    }

    public abstract void Brew();
}

public class EspressoShotDecorator : CoffeeDecorator
{
    public EspressoShotDecorator(ICoffee coffee) : base(coffee)
    {
        Cost = .3M;
        Ingredients.Add("Espresso", 1);
    }

    public override void Brew()
    {
        throw new System.NotImplementedException();
    }
}

public class SyrupDecorator : CoffeeDecorator
{
    public SyrupDecorator(ICoffee coffee) : base(coffee)
    {
        Cost = .5M;
        Ingredients.Add("Hazelnut Syrup", 1);
    }
        
    public override void Brew()
    {
        throw new System.NotImplementedException();
    }
}