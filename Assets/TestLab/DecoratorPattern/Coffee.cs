using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour, ICoffee
{
    [SerializeField] private Liquid liquid;
    
    public decimal Cost { get; set; }
    public Dictionary<string, int> Ingredients { get; set; } = new();
    
    private void Start()
    {
        liquid.ChangeFillAmount(0.71f);
    }

    public void Add(ICoffee coffee) { }
    
    public void Remove(ICoffee coffee) { }

    public void Brew(ICoffee coffee) { }
}
