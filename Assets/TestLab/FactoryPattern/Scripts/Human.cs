using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private Transform handSlot;

    public void EquipWeapon(IWeapon weapon)
    {
        weapon.Equip(handSlot);
    }
}
