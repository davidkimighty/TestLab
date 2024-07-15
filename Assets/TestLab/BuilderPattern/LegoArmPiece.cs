using UnityEngine;

public class LegoArmPiece : LegoPiece<LegoManSet>
{
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null)
        {
            Debug.Log($"[LegoArmPiece] Body piece is missing.");
            return false;
        }

        if (legoSet.ArmLeft != null && legoSet.ArmRight != null)
        {
            Debug.Log($"[LegoArmPiece] No remaining slots.");
            return false;
        }
        
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        legoSet.Body.AddArmPiece(this);
    }
}