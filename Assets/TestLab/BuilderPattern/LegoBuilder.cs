using UnityEngine;

public interface ILegoBuilder<T> where T : LegoSet
{
    ILegoBuilder<T> AddPiece(LegoPiece<T> legoPiece);
    
    T Build();
}

public abstract class LegoBuilder<T> : ILegoBuilder<T> where T : LegoSet
{
    public abstract ILegoBuilder<T> AddPiece(LegoPiece<T> legoPiece);

    public abstract T Build();
}

public class LegoManBuilder : LegoBuilder<LegoManSet>
{
    private LegoManSet legoManSet = new GameObject("LegoMan").AddComponent<LegoManSet>();

    public override ILegoBuilder<LegoManSet> AddPiece(LegoPiece<LegoManSet> legoPiece)
    {
        if (!legoPiece.CanAssemble(legoManSet))
        {
            Debug.Log($"[LegoManBuilder] Unable to assemble {legoPiece.name}");
            return this;
        }

        legoPiece.Assemble(legoManSet);
        return this;
    }

    public override LegoManSet Build()
    {
        return legoManSet;
    }
}
