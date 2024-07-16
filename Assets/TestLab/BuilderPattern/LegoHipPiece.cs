using UnityEngine;

public class LegoHipPiece : LegoPiece<LegoManSet>
{
    public Transform LegSlot;
    
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null)
        {
            return false;
        }

        if (legoSet.Hip != null)
        {
            return false;
        }
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        if (legoSet.Hip == null)
        {
            legoSet.Hip = this;
            transform.SetParent(legoSet.Body.HipSlot);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
