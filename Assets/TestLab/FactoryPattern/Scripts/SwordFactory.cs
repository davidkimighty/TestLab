using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Sword", menuName = "Factory/Sword")]
public class SwordFactory : WeaponFactory
{
    [SerializeField] private Sword swordPrefab;
    
    public override IWeapon CreateWeapon()
    {
        IWeapon weapon = Instantiate(swordPrefab);
        return weapon;
    }
}