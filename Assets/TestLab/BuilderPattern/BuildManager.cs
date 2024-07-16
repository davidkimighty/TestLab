using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Button assembleButton;
    [SerializeField] private Button disassembleButton;

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
        LegoManSet legoMan = legoDirector.Assemble();
    }

    public void Disassemble()
    {
        
    }
}
