using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LegoManSet : LegoSet
{
    private LegoHeadPiece head;
    private LegoBodyPiece body;
    private LegoHipPiece hip;
    private List<LegoArmPiece> arms = new();
    private List<LegoHandPiece> hands = new();
    private List<LegoLegPiece> legs = new();
    
    public override void Disassemble()
    {
        head.Disassemble();
        body.Disassemble();
        hip.Disassemble();
        
        foreach (LegoArmPiece arm in arms)
            arm.Disassemble();
        
        foreach (LegoHandPiece hand in hands)
            hand.Disassemble();
        
        foreach (LegoLegPiece leg in legs)
            leg.Disassemble();

        head = null;
        body = null;
        hip = null;
        arms.Clear();
        hands.Clear();
        legs.Clear();
    }

    public bool AddHead(LegoHeadPiece head, out Transform target)
    {
        target = null;
        if (this.head != null || body == null) return false;

        this.head = head;
        target = body.HeadSlot;
        return true;
    }

    public bool AddBody(LegoBodyPiece body, out Transform target)
    {
        target = null;
        if (this.body != null) return false;

        this.body = body;
        target = transform;
        return true;
    }

    public bool AddHip(LegoHipPiece hip, out Transform target)
    {
        target = null;
        if (this.hip != null || body == null) return false;

        this.hip = hip;
        target = body.HipSlot;
        return true;
    }

    public bool AddArm(LegoArmPiece arm, out Transform target)
    {
        target = null;
        if (arms.Count == 2 || body == null) return false;

        arms.Add(arm);
        Slot slot = !body.LeftArmSlot.IsFull ? body.LeftArmSlot : body.RightArmSlot;
        slot.IsFull = true;
        target = slot.Anchor;
        return true;
    }
    
    public bool AddHand(LegoHandPiece hand, out Transform target)
    {
        target = null;
        if (hands.Count == 2 || arms.Count == 0 ||
            arms.All(arm => arm.HandSlot.IsFull)) return false;

        hands.Add(hand);
        LegoArmPiece arm = arms.First(arm => !arm.HandSlot.IsFull);
        arm.HandSlot.IsFull = true;
        target = arm.HandSlot.Anchor;
        return true;
    }

    public bool AddLeg(LegoLegPiece leg, out Transform target)
    {
        target = null;
        if (legs.Count == 2 || hip == null) return false;

        legs.Add(leg);
        target = hip.LegSlot;
        return true;
    }
}
