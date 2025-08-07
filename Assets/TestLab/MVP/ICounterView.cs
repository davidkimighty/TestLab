using System;

public interface ICounterView
{
    event Action OnCountClick;

    void UpdateCount(int count);
}