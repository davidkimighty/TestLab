using System;
using System.Collections.Generic;
using UnityEngine;

public class LegoDirector<T> where T : LegoSet
{
    private ILegoBuilder<T> builder;
    private List<Action<ILegoBuilder<T>>> buildSteps;

    public LegoDirector(ILegoBuilder<T> builder)
    {
        this.builder = builder;
        buildSteps = new List<Action<ILegoBuilder<T>>>();
    }

    public void AddBuildStep(Action<ILegoBuilder<T>> step)
    {
        buildSteps.Add(step);
    }
    
    public T Assemble()
    {
        foreach (Action<ILegoBuilder<T>> step in buildSteps)
            step(builder);
        
        return builder.Build();
    }
}
 