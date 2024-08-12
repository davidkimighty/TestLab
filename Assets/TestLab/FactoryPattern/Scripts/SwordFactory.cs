using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Sword", menuName = "Factory/Sword")]
public class SwordFactory : WeaponFactory
{
    [SerializeField] private Sword swordPrefab;
    
    public override IWeapon Create()
    {
        IWeapon weapon = Instantiate(swordPrefab);
        return weapon;
    }
}