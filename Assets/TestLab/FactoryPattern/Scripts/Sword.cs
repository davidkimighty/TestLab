using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform gripPoint;

    public void Attack()
    {
        
    }

    public void Equip(Transform anchorPoint)
    {
        transform.parent = anchorPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}