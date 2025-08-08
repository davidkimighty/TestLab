using System;

public class HealthModel
{
    public event Action<int> OnHealthChanged;

    private int _health;
    public int Health => _health;

    public HealthModel(int health)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
            return;
        }
        OnHealthChanged?.Invoke(_health);
    }
}
