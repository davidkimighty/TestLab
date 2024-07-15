using UnityEngine;

public abstract class LegoPiece<T> : MonoBehaviour where T : LegoSet
{
    public abstract bool CanAssemble(T legoSet);

    public abstract void Assemble(T legoSet);
}
