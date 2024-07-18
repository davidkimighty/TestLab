using UnityEngine;

public class LegoHipPiece : LegoPiece<LegoManSet>
{
    public Transform LegSlot;
    
    public override void Assemble(LegoManSet legoSet)
    {
        if (smoothAssembleCoroutine != null) return;
        
        if (!legoSet.AddHip(this, out Transform target)) return;
        
        body.isKinematic = true;
        transform.SetParent(target);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        if (smoothAssembleCoroutine != null) return;
        
        if (!legoSet.AddHip(this, out Transform target)) return;
        
        smoothAssembleCoroutine = SmoothAssemble(target, smoothAssembleDuration, delay);
        StartCoroutine(smoothAssembleCoroutine);
    }
}
