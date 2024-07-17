using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    private LegoBuilder<LegoManSet> legoManBuilder;
    private LegoDirector<LegoManSet> legoDirector;
    
    private void Start()
    {
        legoManBuilder = new LegoManBuilder();
        legoDirector = new LegoDirector<LegoManSet>(legoManBuilder);
        legoDirector.AddBuildSteps(legoManBuilder.GetAssembleOrder());
    }

    public void Assemble()
    {
        LegoManSet legoMan = legoDirector.SmoothAssemble(0.3f);
    }

    public void Disassemble()
    {
        
    }
}
