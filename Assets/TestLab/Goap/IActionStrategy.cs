using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public interface IActionStrategy
{
    bool CanPerform { get; }
    bool Complete { get; }

    void Start() { }
    void Update(float deltaTime) { }
    void Stop() { }
}

public class IdleStrategy : IActionStrategy
{
    private Sheep _sheep;
    private float _duration;
    private float _elapsedTime;
    
    public bool CanPerform => true;
    public bool Complete { get; private set; }

    public IdleStrategy(Sheep sheep, float duration)
    {
        _sheep = sheep;
        _duration = duration;
    }

    public void Start()
    {
        Complete = false;
        _sheep.IdleAnimation();
    }

    public void Update(float deltaTime)
    {
        _elapsedTime += deltaTime;
        
        if (_elapsedTime > _duration)
        {
            _sheep.Resting();
            Complete = true;
            _elapsedTime = 0f;
        }
    }
}

public class WanderStrategy : IActionStrategy
{
    private Sheep _sheep;
    private readonly NavMeshAgent _agent;
    private readonly float _wanderRadius;
    
    public bool CanPerform => !Complete;
    public bool Complete => _agent.remainingDistance <= 0.1f && !_agent.pathPending;

    public WanderStrategy(Sheep sheep, NavMeshAgent agent, float wanderRadius)
    {
        _sheep = sheep;
        _agent = agent;
        _wanderRadius = wanderRadius;
    }

    public void Start()
    {
        _sheep.BounceAnimation();
        
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = (Random.insideUnitSphere * _wanderRadius);
            randomDirection.y = 0;

            if (NavMesh.SamplePosition(_sheep.transform.position + randomDirection, out NavMeshHit hit, _wanderRadius, 1))
            {
                Debug.Log("Destination set");
                _agent.SetDestination(hit.position);
                return;
            }
        }
    }
    
    public void Update(float deltaTime)
    {
        _sheep.Moving();
    }
}

public class MoveStrategy : IActionStrategy
{
    private Sheep _sheep;
    private readonly NavMeshAgent _agent;
    private readonly Func<Vector3> _destination;

    public bool CanPerform => !Complete;
    public bool Complete => _agent.remainingDistance <= 0.1f && !_agent.pathPending;

    public MoveStrategy(Sheep sheep, NavMeshAgent agent, Func<Vector3> destination)
    {
        _sheep = sheep;
        _agent = agent;
        _destination = destination;
    }

    public void Start()
    {
        _sheep.BounceAnimation();
        _agent.SetDestination(_destination());
    }

    public void Update(float deltaTime)
    {
        _sheep.Moving();
    }

    public void Stop()
    {
        _agent.ResetPath();
    }
}

public class EatStrategy : IActionStrategy
{
    private Sheep _sheep;
    private Bush _bush;
    
    public bool CanPerform => !Complete && _bush.CanEat();
    public bool Complete { get; private set; }

    public EatStrategy(Sheep sheep, Bush bush)
    {
        _sheep = sheep;
        _bush = bush;
    }
    
    public void Start()
    {
        Complete = false;
        _sheep.EatAnimation();
    }

    public void Update(float deltaTime)
    {
        if (!_sheep.Eating(deltaTime, _bush))
            Complete = true;
    }
    
    public void Stop()
    {
        _sheep.IdleAnimation();
    }
}