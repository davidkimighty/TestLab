using System.Collections.Generic;

public interface ICoffee
{
    decimal Cost { get; set; }
    Dictionary<string, int> Ingredients { get; set; }
    
    void Add();

    void Remove();

    void Brew();
}