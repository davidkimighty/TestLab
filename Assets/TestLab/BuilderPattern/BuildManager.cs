using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private LegoDirector<LegoManSet> legoDirector;
    
    private void Start()
    {
        legoDirector = new LegoDirector<LegoManSet>(new LegoManBuilder());
        //legoDirector.AddBuildStep(s => s.AddPiece());
        LegoManSet newLegoMan = legoDirector.Assemble();
    }
}
