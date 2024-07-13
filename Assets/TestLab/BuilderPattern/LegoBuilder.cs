using UnityEngine;

public interface ILegoBuilder<out T> where T : LegoSet
{
    T Build();
}

public abstract class LegoBuilder<T> : ILegoBuilder<T> where T : LegoSet
{
    public abstract T Build();
}

public class LegoManBuilder : LegoBuilder<LegoMan>
{
    public override LegoMan Build()
    {
        throw new System.NotImplementedException();
    }
}
