using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILegoBuilder<T> where T : LegoSet
{
    List<LegoPiece<T>> GetAssembleOrder();
    
    ILegoBuilder<T> AssemblePiece(LegoPiece<T> legoPiece);
    
    ILegoBuilder<T> AssemblePieceAfterIncrementalDelay(LegoPiece<T> legoPiece, float delay);
    
    T Build();
}

public abstract class LegoBuilder<T> : ILegoBuilder<T> where T : LegoSet
{
    public abstract List<LegoPiece<T>> GetAssembleOrder();

    public abstract ILegoBuilder<T> AssemblePiece(LegoPiece<T> legoPiece);
    
    public abstract ILegoBuilder<T> AssemblePieceAfterIncrementalDelay(LegoPiece<T> legoPiece, float delay);

    public abstract T Build();
}

public class LegoManBuilder : LegoBuilder<LegoManSet>
{
    private LegoManSet legoManSet = new GameObject("LegoMan").AddComponent<LegoManSet>();
    private float incrementalDelay = 0f;
    
    public override List<LegoPiece<LegoManSet>> GetAssembleOrder()
    {
        var pieces = Object.FindObjectsByType<LegoPiece<LegoManSet>>(FindObjectsSortMode.None);
        List<LegoPiece<LegoManSet>> sorted = pieces.OrderBy(p => p.Priority).ToList();
        return sorted;
    }
    
    public override ILegoBuilder<LegoManSet> AssemblePiece(LegoPiece<LegoManSet> legoPiece)
    {
        if (!legoPiece.CanAssemble(legoManSet))
        {
            Debug.Log($"[LegoManBuilder] Unable to assemble {legoPiece.name}");
            return this;
        }

        legoPiece.Assemble(legoManSet);
        return this;
    }

    public override ILegoBuilder<LegoManSet> AssemblePieceAfterIncrementalDelay(LegoPiece<LegoManSet> legoPiece, float delay)
    {
        if (!legoPiece.CanAssemble(legoManSet))
        {
            Debug.Log($"[LegoManBuilder] Unable to assemble {legoPiece.name}");
            return this;
        }

        incrementalDelay += delay;
        legoPiece.AssembleAfterDelay(legoManSet, incrementalDelay);
        return this;
    }

    public override LegoManSet Build()
    {
        return legoManSet;
    }
}
