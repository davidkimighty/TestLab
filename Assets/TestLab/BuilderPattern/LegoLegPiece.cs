using UnityEngine;

public class LegoLegPiece : LegoPiece<LegoManSet>
{
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Hip == null)
        {
            return false;
        }

        if (legoSet.LegLeft != null && legoSet.LegRight != null)
        {
            return false;
        } 
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        if (legoSet.LegLeft == null)
            legoSet.LegLeft = this;
        else
            legoSet.LegRight = this;
        
        transform.SetParent(legoSet.Hip.LegSlot);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
