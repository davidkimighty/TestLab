using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class DummyExecutor : MonoBehaviour
{
    [SerializeField] private int _loopCount = 1000;
    [SerializeField] private bool _useJobSystem;
    [SerializeField] private int _numJobs = 3;
    
    [SerializeField] private TMP_Text _frameText;
    [SerializeField] private TMP_Text _typeText;

    private IEnumerator _dummyExecuteCoroutine;
    private JobHandle _jobHandle;
    private NativeList<JobHandle> _jobHandles;
    
    private int _frameIndex;
    private float[] _frameDeltaTimes;

    public void Switch()
    {
        _useJobSystem = !_useJobSystem;
        if (_dummyExecuteCoroutine != null)
            StopCoroutine(_dummyExecuteCoroutine);
        _dummyExecuteCoroutine = _useJobSystem ? PerformDummyJob() : PerformDummy();
        StartCoroutine(_dummyExecuteCoroutine);
        _typeText.text = $"Job System [{(_useJobSystem ? "enabled" : "disabled")}]";
    }
    
    private void Start()
    {
        _dummyExecuteCoroutine = _useJobSystem ? PerformDummyJob() : PerformDummy();
        StartCoroutine(_dummyExecuteCoroutine);
        _typeText.text = $"Job System [{(_useJobSystem ? "enabled" : "disabled")}]";
        
        _frameDeltaTimes = new float[50];
    }

    private void Update()
    {
        _frameDeltaTimes[_frameIndex] = Time.unscaledDeltaTime;
        _frameIndex = (_frameIndex + 1) % _frameDeltaTimes.Length;
        _frameText.text = $"FPS {Mathf.RoundToInt(GetAverageFPS())}";
    }

    private IEnumerator PerformDummyJob()
    {
        while (true)
        {
            DummyJob job = new(_loopCount);
            _jobHandle = job.Schedule();
            
            yield return new WaitForEndOfFrame();
            _jobHandle.Complete();
        }
    }

    private IEnumerator PerformDummyJobList()
    {
        int baseNum = _loopCount / _numJobs;
        int remainNum = _loopCount % _numJobs;

        while (true)
        {
            _jobHandles = new NativeList<JobHandle>(Allocator.Temp);
            for (int i = 0; i < _numJobs; i++)
                _jobHandles.Add(new DummyJob(baseNum + (i < remainNum ? 1 : 0)).Schedule());
            
            JobHandle.CompleteAll(_jobHandles.AsArray()); // call right away?
            yield return null;
        }
    }
    
    private IEnumerator PerformDummy()
    {
        while (true)
        {
            DummyJob.Dummy(_loopCount);
            yield return null;
        }
    }
    
    private float GetAverageFPS()
    {
        float fpsTotal = 0f;
        foreach (float t in _frameDeltaTimes)
            fpsTotal += t;
        return _frameDeltaTimes.Length / fpsTotal;
    }
}
