using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct DummyJob : IJob
{
    private int _loop;
    
    public DummyJob(int loop)
    {
        _loop = loop;
    }

    public static void Dummy(int loop)
    {
        for (int i = 0; i < loop; i++)
            _ = Mathf.Sqrt(Mathf.Pow(10f, 100000f) / 10000000f);
    }
    
    public void Execute()
    {
        Dummy(_loop);
    }
}
