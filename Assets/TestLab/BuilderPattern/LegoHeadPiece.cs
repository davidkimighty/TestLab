using UnityEngine;

public class LegoHeadPiece : LegoPiece<LegoManSet>
{
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null) return false;
        
        if (smoothAssembleCoroutine != null) return false;
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        legoSet.Head = this;
        
        body.isKinematic = true;
        transform.SetParent(legoSet.Body.HeadSlot);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        legoSet.Head = this;
        
        smoothAssembleCoroutine = SmoothAssemble(legoSet.Body.HeadSlot, smoothAssembleDuration, delay);
        StartCoroutine(smoothAssembleCoroutine);
    }
}
