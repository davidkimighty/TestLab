using System;

public interface ICounterView
{
    event Action OnCount;

    void UpdateCount(int count);
}