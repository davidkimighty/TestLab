using System;

public interface IHealthView
{
    event Action OnTakeDamage;

    void Initialize(HealthModel model);
}
