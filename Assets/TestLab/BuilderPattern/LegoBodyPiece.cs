using UnityEngine;

public class LegoBodyPiece : LegoPiece<LegoManSet>
{
    [SerializeField] private Transform headSlot;
    [SerializeField] private Transform hipSlot;
    [SerializeField] private Transform leftArmSlot;
    [SerializeField] private Transform rightArmSlot;
    
    public override bool CanAssemble(LegoManSet set)
    {
        throw new System.NotImplementedException();
    }

    public override void Assemble(LegoManSet legoSet)
    {
        throw new System.NotImplementedException();
    }

    public void AddArmPiece(LegoArmPiece armPiece)
    {
        
    }
}
