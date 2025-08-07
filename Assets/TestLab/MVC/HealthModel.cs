using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthModel", menuName = "TestLab/MVC/HealthModel")]
public class HealthModel : ScriptableObject
{
    public event Action<int> OnHealthChanged;

    public int Health { get; private set; } = 100;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
            Health = 0;
        OnHealthChanged?.Invoke(Health);
    }
}
