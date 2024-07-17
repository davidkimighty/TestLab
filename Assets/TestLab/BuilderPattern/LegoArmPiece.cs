using UnityEngine;

public class LegoArmPiece : LegoPiece<LegoManSet>
{
    public Transform HandSlot;
    
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null)
        {
            Debug.Log($"[LegoArmPiece] Body piece is missing.");
            return false;
        }

        if (legoSet.ArmLeft != null && legoSet.ArmRight != null) return false;
        
        if (smoothAssembleCoroutine != null) return false;
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        if (legoSet.ArmLeft == null)
        {
            legoSet.ArmLeft = this;
            transform.SetParent(legoSet.Body.LeftArmSlot);
        }
        else
        {
            legoSet.ArmRight = this;
            transform.SetParent(legoSet.Body.RightArmSlot);
        }
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        if (legoSet.ArmLeft == null)
        {
            legoSet.ArmLeft = this;
            smoothAssembleCoroutine = SmoothAssemble(legoSet.Body.LeftArmSlot, smoothAssembleDuration, delay);
        }
        else
        {
            legoSet.ArmRight = this;
            smoothAssembleCoroutine = SmoothAssemble(legoSet.Body.RightArmSlot, smoothAssembleDuration, delay);
        }
        StartCoroutine(smoothAssembleCoroutine);
    }
}