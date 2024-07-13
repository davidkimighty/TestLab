using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private LegoMan legoMan;
    
    private LegoDirector<LegoMan> legoDirector;
    
    private void Start()
    {
        legoDirector = new LegoDirector<LegoMan>(new LegoManBuilder());
        LegoMan newLegoMan = legoDirector.Assemble(legoMan);
    }
}
