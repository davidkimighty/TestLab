using System;
using System.Collections.Generic;

[Serializable]
public class BrewOperation
{
    public int Count;
    public Action<Liquid> Execution;

    public BrewOperation(int count, Action<Liquid> execution)
    {
        Count = count;
        Execution = execution;
    }
}

public interface ICoffee
{
    decimal Cost { get; set; }
    Dictionary<string, BrewOperation> Ingredients { get; set; }
    
    void Add();

    void Remove();
}