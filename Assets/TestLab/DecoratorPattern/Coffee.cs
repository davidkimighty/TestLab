using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour, ICoffee
{
    [SerializeField] private Liquid liquid;
    
    public decimal Cost { get; set; }
    public Dictionary<string, int> Ingredients { get; set; } = new();
    
    public void Add()
    {
        
    }

    public void Remove()
    {
        
    }

    public void Brew()
    {
        
    }

    private void Start()
    {
        liquid.ChangeFillAmount(0.71f);
    }

}
