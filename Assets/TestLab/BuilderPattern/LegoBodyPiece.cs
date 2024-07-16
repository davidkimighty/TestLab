using UnityEngine;

public class LegoBodyPiece : LegoPiece<LegoManSet>
{
    public Transform HeadSlot;
    public Transform HipSlot;
    public Transform LeftArmSlot;
    public Transform RightArmSlot;
    
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body != null)
        {
            return false;
        }
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        legoSet.Body = this;
        transform.SetParent(legoSet.transform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
