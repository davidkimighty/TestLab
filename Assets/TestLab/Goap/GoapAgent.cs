using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour
{
    public float Hunger = 100f;
    public float Stamina = 100f;

    [SerializeField] private Sensor _findFoodSensor;
    [SerializeField] private Transform _bush;
    
    private NavMeshAgent _navMeshAgent;
    private GameObject _target;
    private Vector3 _destination;

    private AgentGoal _previousGoal;
    public AgentGoal CurrentGoal;
    public ActionPlan ActionPlan;
    public AgentAction CurrentAction;

    public Dictionary<string, AgentBelief> Beliefs;
    public HashSet<AgentAction> Actions;
    public HashSet<AgentGoal> Goals;
    private IGoapPlanner _planner;
    private float _statTime = 0f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _planner = new GoapPlanner();
    }
    
    private void OnEnable()
    {
        _findFoodSensor.OnTargetChanged += HandleTargetChanged;
    }

    private void OnDisable()
    {
        _findFoodSensor.OnTargetChanged -= HandleTargetChanged;
    }

    private void Start()
    {
        SetupBeliefs();
        SetupActions();
        SetupGoals();
    }

    private void Update()
    {
        UpdateStats();
        
        if (CurrentAction == null)
        {
            CalculatePlan();

            if (ActionPlan != null && ActionPlan.Actions.Count > 0) 
            {
                _navMeshAgent.ResetPath();

                CurrentGoal = ActionPlan.AgentGoal;
                Debug.Log($"Goal: {CurrentGoal.Name}, plans: {ActionPlan.Actions.Count}");
                CurrentAction = ActionPlan.Actions.Pop();
                Debug.Log($"Popped action: {CurrentAction.Name}");
                CurrentAction.Start();
            }
        }

        if (ActionPlan != null && CurrentAction != null)
        {
            CurrentAction.Update(Time.deltaTime);
            if (CurrentAction.Complete)
            {
                Debug.Log($"{CurrentAction.Name} complete");
                CurrentAction.Stop();
                CurrentAction = null;
                if (ActionPlan.Actions.Count == 0)
                {
                    Debug.Log($"plan complete");
                    _previousGoal = CurrentGoal;
                    CurrentGoal = null;
                }
            }
        }
    }

    private void HandleTargetChanged()
    {
        Debug.Log("Target changed.");
        CurrentAction = null;
        CurrentGoal = null;
    }
    
    private void SetupBeliefs()
    {
        Beliefs = new Dictionary<string, AgentBelief>();
        BeliefFactory factory = new BeliefFactory(this, Beliefs);
        
        factory.AddBelief("Nothing", () => false);
        factory.AddBelief("AgentIdle", () => !_navMeshAgent.hasPath);
        factory.AddBelief("AgentMoving", () => _navMeshAgent.hasPath);
        factory.AddBelief("AgentStaminaLow", () => Stamina < 10);
        factory.AddBelief("AgentStaminaHigh", () => Stamina >= 50);
        factory.AddBelief("AgentHungerLow", () => Hunger < 10);
        factory.AddBelief("AgentHungerHigh", () => Hunger >= 50);
        
        factory.AddLocationBelief("AgentAtBush", 0.1f, _bush);
    }
    
    private void SetupActions()
    {
        Actions = new HashSet<AgentAction>();
        Actions.Add(new AgentAction.Builder("Relax", this)
            .WithStrategy(new IdleStrategy(5))
            .AddEffect(Beliefs["Nothing"])
            .Build());
        
        Actions.Add(new AgentAction.Builder("WanderAround", this)
            .WithStrategy(new WanderStrategy(_navMeshAgent, 1))
            .AddEffect(Beliefs["AgentMoving"])
            .Build());
        
        Actions.Add(new AgentAction.Builder("MoveToEat", this)
            .WithStrategy(new MoveStrategy(_navMeshAgent, () => _bush.position))
            .AddEffect(Beliefs["AgentAtBush"])
            .Build());

        Actions.Add(new AgentAction.Builder("Eat", this)
            .WithStrategy(new IdleStrategy(3f))
            .AddPrecondition(Beliefs["AgentAtBush"])
            .AddEffect(Beliefs["AgentHungerHigh"])
            .Build());
    }
    
    private void SetupGoals()
    {
        Goals = new HashSet<AgentGoal>();
        Goals.Add(new AgentGoal.Builder("Chill Out")
            .WithPriority(1)
            .WithDesiredEffect(Beliefs["Nothing"])
            .Build());
        
        Goals.Add(new AgentGoal.Builder("Wander")
            .WithPriority(1)
            .WithDesiredEffect(Beliefs["AgentMoving"])
            .Build());
        
        Goals.Add(new AgentGoal.Builder("NotBeHungry")
            .WithPriority(3)
            .WithDesiredEffect(Beliefs["AgentHungerHigh"])
            .Build());
    }
    
    private void CalculatePlan()
    {
        var priorityLevel = CurrentGoal?.Priority ?? 0;

        HashSet<AgentGoal> goalsToCheck = Goals;

        if (CurrentGoal != null)
        {
            goalsToCheck = new HashSet<AgentGoal>(Goals.Where(g => g.Priority > priorityLevel));
        }

        var potentialPlan = _planner.Plan(this, goalsToCheck, _previousGoal);
        if (potentialPlan != null)
            ActionPlan = potentialPlan;
    }

    private void UpdateStats()
    {
        _statTime += Time.deltaTime;
        if (_statTime > 2f)
        {
            Hunger += Vector3.Distance(transform.position, _bush.position) < 0.15f ? 20f : -5f;
            _statTime = 0f;
        }
    }
}
