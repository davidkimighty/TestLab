using UnityEngine;

public class LegoDirector<T> where T : LegoSet
{
    private ILegoBuilder<T> builder;

    public LegoDirector(ILegoBuilder<T> builder)
    {
        this.builder = builder;
    }
    
    public T Assemble(T legoSet)
    {
        return builder.Build();
    }
}
 