using UnityEngine;

public interface IWeapon
{
    void Attack();

    void Equip(Transform anchorPoint);
}

public abstract class WeaponFactory : ScriptableObject
{
    public abstract IWeapon Create();
}