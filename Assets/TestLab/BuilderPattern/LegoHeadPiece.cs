using UnityEngine;

public class LegoHeadPiece : LegoPiece<LegoManSet>
{
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null)
        {
            return false;
        }
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        if (legoSet.Head == null)
        {
            legoSet.Head = this;
            transform.SetParent(legoSet.Body.HeadSlot);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
