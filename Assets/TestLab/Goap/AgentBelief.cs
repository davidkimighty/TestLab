using System;
using System.Collections.Generic;
using UnityEngine;

public class BeliefFactory
{
    private readonly GoapAgent _agent;
    private readonly Dictionary<string, AgentBelief> _beliefs;

    public BeliefFactory(GoapAgent agent, Dictionary<string, AgentBelief> beliefs)
    {
        _agent = agent;
        _beliefs = beliefs;
    }

    public void AddBelief(string key, Func<bool> condition)
    {
        _beliefs.Add(key, new AgentBelief.Builder(key)
            .WithCondition(condition)
            .Build());
    }

    public void AddSensorBelief(string key, Sensor sensor)
    {
        _beliefs.Add(key, new AgentBelief.Builder(key)
            .WithCondition(() => sensor.IsTargetInRange)
            .WithLocation(() => sensor.TargetPosition)
            .Build());
    }
    
    public void AddLocationBelief(string key, float distance, Transform locationCondition)
    {
        AddLocationBelief(key, distance, locationCondition.position);
    }
    
    public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
    {
        _beliefs.Add(key, new AgentBelief.Builder(key)
            .WithCondition(() => InRangeOf(locationCondition, distance))
            .WithLocation(() => locationCondition)
            .Build());
    }

    private bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(_agent.transform.position, pos) < range;
}

public class AgentBelief
{
    private Func<bool> _condition = () => false;
    private Func<Vector3> _observedLocation = () => Vector3.zero;
    
    public string Name { get; }

    public Vector3 Location => _observedLocation();

    AgentBelief(string name)
    {
        Name = name;
    }

    public bool Evaluate() => _condition();

    public class Builder
    {
        private readonly AgentBelief _belief;

        public Builder(string name)
        {
            _belief = new AgentBelief(name);
        }

        public Builder WithCondition(Func<bool> condition)
        {
            _belief._condition = condition;
            return this;
        }

        public Builder WithLocation(Func<Vector3> observedLocation)
        {
            _belief._observedLocation = observedLocation;
            return this;
        }

        public AgentBelief Build() => _belief;
    }
}
