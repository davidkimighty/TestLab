using System.Collections.Generic;
using UnityEngine;

namespace TestLab.DecoratorPattern
{
    public abstract class CoffeeDecorator : ICoffee
    {
        protected ICoffee coffee;
        protected string name;
        
        public decimal Cost { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }

        protected CoffeeDecorator(string name)
        {
            this.name = name;
        }
        
        public virtual void Add(ICoffee coffee)
        {
            coffee.Cost += Cost;
            coffee.Ingredients[name] += 1;
        }

        public virtual void Remove(ICoffee coffee)
        {
            if (!coffee.Ingredients.ContainsKey(name)) return;
            
            coffee.Cost -= Cost;
            int count = coffee.Ingredients[name] - 1;
            coffee.Ingredients[name] = count < 0 ? 0 : count;
        }

        public abstract void Brew(ICoffee coffee);
    }

    public class EspressoShotDecorator : CoffeeDecorator
    {
        public EspressoShotDecorator(string name) : base(name)
        {
            Cost = .3M;
            Ingredients.Add(name, 1);
        }

        public override void Brew(ICoffee coffee)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SyrupDecorator : CoffeeDecorator
    {
        public SyrupDecorator(string name) : base(name)
        {
            Cost = .5M;
            Ingredients.Add(name, 1);
        }
        
        public override void Brew(ICoffee coffee)
        {
            throw new System.NotImplementedException();
        }
    }
}