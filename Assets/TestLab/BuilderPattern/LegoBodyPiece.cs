using UnityEngine;

public class LegoBodyPiece : LegoPiece<LegoManSet>
{
    public Transform HeadSlot;
    public Transform HipSlot;
    public Slot LeftArmSlot;
    public Slot RightArmSlot;

    public override void Assemble(LegoManSet legoSet)
    {
        if (smoothAssembleCoroutine != null) return;
        
        if (!legoSet.AddBody(this, out Transform target)) return;

        body.isKinematic = true;
        transform.SetParent(target);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void AssembleAfterDelay(LegoManSet legoSet, float delay)
    {
        if (smoothAssembleCoroutine != null) return;
        
        if (!legoSet.AddBody(this, out Transform target)) return;
        
        smoothAssembleCoroutine = SmoothAssemble(target, smoothAssembleDuration, 0f, false);
        StartCoroutine(smoothAssembleCoroutine);
    }

    public override void Disassemble()
    {
        base.Disassemble();

        LeftArmSlot.IsFull = false;
        RightArmSlot.IsFull = false;
    }
}
