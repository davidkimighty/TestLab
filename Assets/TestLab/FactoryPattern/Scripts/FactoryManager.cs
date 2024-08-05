using System;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    [SerializeField] private WeaponFactory weaponFactory;
    [SerializeField] private Human human;
    [SerializeField] private Zombie zom;

    private IWeapon weapon;

    private void Start()
    {
        weapon = weaponFactory.CreateWeapon();
        human.EquipWeapon(weapon);
    }
}
