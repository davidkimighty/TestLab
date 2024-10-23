using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public interface IActionStrategy
{
    bool CanPerform { get; }
    bool Complete { get; }

    void Start(MonoBehaviour mono) { }
    void Update(float deltaTime) { }
    void Stop() { }
}

public class IdleStrategy : IActionStrategy
{
    private IEnumerator _strategy;
    private float _duration;
    public bool CanPerform => true;
    public bool Complete { get; private set; }

    public IdleStrategy(float duration)
    {
        _duration = duration;
    }

    public void Start(MonoBehaviour mono)
    {
        if (_strategy != null)
            mono.StopCoroutine(_strategy);
        _strategy = Timer(_duration);
        mono.StartCoroutine(_strategy);
    }
    
    private IEnumerator Timer(float duration)
    {
        float elapsedTime = 0f;
        Complete = false;
        
        while (duration > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Complete = true;
    }
}

public class WanderStrategy : IActionStrategy
{
    private readonly NavMeshAgent _agent;
    private readonly float _wanderRadius;
    
    public bool CanPerform => !Complete;
    public bool Complete => _agent.remainingDistance <= 0.1f && !_agent.pathPending;

    public WanderStrategy(NavMeshAgent agent, float wanderRadius)
    {
        _agent = agent;
        _wanderRadius = wanderRadius;
    }

    public void Start(MonoBehaviour mono)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = (Random.insideUnitSphere * _wanderRadius);
            randomDirection.y = 0;

            if (NavMesh.SamplePosition(_agent.transform.position + randomDirection, out NavMeshHit hit, _wanderRadius, 1))
            {
                Debug.Log("Destination set");
                _agent.SetDestination(hit.position);
                return;
            }
        }
    }
}

public class MoveStrategy : IActionStrategy
{
    private readonly NavMeshAgent _agent;
    private readonly Func<Vector3> _destination;

    public bool CanPerform => !Complete;
    public bool Complete => _agent.remainingDistance <= 0.1f && !_agent.pathPending;

    public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination)
    {
        _agent = agent;
        _destination = destination;
    }

    public void Start(MonoBehaviour mono)
    {
        _agent.SetDestination(_destination());
    }

    public void Stop()
    {
        _agent.ResetPath();
    }
}