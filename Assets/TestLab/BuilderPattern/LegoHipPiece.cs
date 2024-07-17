using UnityEngine;

public class LegoHipPiece : LegoPiece<LegoManSet>
{
    public Transform LegSlot;
    
    public override bool CanAssemble(LegoManSet legoSet)
    {
        if (legoSet.Body == null) return false;

        if (legoSet.Hip != null) return false;
        
        if (smoothAssembleCoroutine != null) return false;
        return true;
    }

    public override void Assemble(LegoManSet legoSet)
    {
        legoSet.Hip = this;
        
        body.isKinematic = true;
        transform.SetParent(legoSet.Body.HipSlot);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        legoSet.Hip = this;
        
        smoothAssembleCoroutine = SmoothAssemble(legoSet.Body.HipSlot, smoothAssembleDuration, delay);
        StartCoroutine(smoothAssembleCoroutine);
    }
}
