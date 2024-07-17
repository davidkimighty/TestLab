using UnityEngine;

public class LegoHandPiece : LegoPiece<LegoManSet>
{
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.ArmLeft == null && legoSet.ArmRight == null) return false;

        if (legoSet.HandLeft != null && legoSet.HandRight != null) return false;
        
        if (smoothAssembleCoroutine != null) return false;
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        if (legoSet.HandLeft == null)
        {
            legoSet.HandLeft = this;
            transform.SetParent(legoSet.ArmLeft.HandSlot);
        }
        else
        {
            legoSet.HandRight = this;
            transform.SetParent(legoSet.ArmRight.HandSlot);
        }
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        if (legoSet.HandLeft == null)
        {
            legoSet.HandLeft = this;
            smoothAssembleCoroutine = SmoothAssemble(legoSet.ArmLeft.HandSlot, smoothAssembleDuration, delay);
        }
        else
        {
            legoSet.HandRight = this;
            smoothAssembleCoroutine = SmoothAssemble(legoSet.ArmRight.HandSlot, smoothAssembleDuration, delay);
        }
        StartCoroutine(smoothAssembleCoroutine);
    }
}
