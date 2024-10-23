using System.Collections.Generic;
using UnityEngine;

public class AgentAction
{
    public string Name { get; }
    public float Cost { get; private set; }

    public HashSet<AgentBelief> Preconditions { get; } = new();
    public HashSet<AgentBelief> Effects { get; } = new();

    private IActionStrategy _strategy;

    public bool Complete => _strategy.Complete;
    private MonoBehaviour _mono;

    private AgentAction(string name, MonoBehaviour mono)
    {
        Name = name;
        _mono = mono;
    }

    public void Start() => _strategy.Start(_mono);

    public void Update(float delaTime)
    {
        if (_strategy.CanPerform)
            _strategy.Update(delaTime);

        if (!_strategy.Complete) return;

        foreach (var effect in Effects)
            effect.Evaluate();
    }

    public void Stop() => _strategy.Stop();

    public class Builder
    {
        private readonly AgentAction _action;

        public Builder(string name, MonoBehaviour mono)
        {
            _action = new AgentAction(name, mono)
            {
                Cost = 1
            };
        }

        public Builder WithCost(float cost)
        {
            _action.Cost = cost;
            return this;
        }

        public Builder WithStrategy(IActionStrategy strategy)
        {
            _action._strategy = strategy;
            return this;
        }

        public Builder AddPrecondition(AgentBelief precondition)
        {
            _action.Preconditions.Add(precondition);
            return this;
        }

        public Builder AddEffect(AgentBelief effect)
        {
            _action.Effects.Add(effect);
            return this;
        }

        public AgentAction Build() => _action;
    }
}