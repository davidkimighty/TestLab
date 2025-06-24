using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoapAgent : MonoBehaviour
{
    [SerializeField] private Sheep _sheep;
    [SerializeField] private Bush _bush;
    
    [SerializeField] private TMP_Text _goalText;
    [SerializeField] private TMP_Text _planText;
    
    private NavMeshAgent _navMeshAgent;
    private IGoapPlanner _planner;

    private AgentGoal _previousGoal;
    private AgentGoal _currentGoal;
    private ActionPlan _actionPlan;
    private AgentAction _currentAction;

    public Dictionary<string, AgentBelief> Beliefs;
    public HashSet<AgentAction> Actions;
    public HashSet<AgentGoal> Goals;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _planner = new GoapPlanner();
    }
    
    private void Start()
    {
        SetupBeliefs();
        SetupActions();
        SetupGoals();
    }

    private void Update()
    {
        if (_currentAction == null)
        {
            CalculatePlan();
            
            _goalText.text = _currentGoal == null ? "Goal: " : $"Goal: {_currentGoal.Name}";
            StringBuilder builder = new();
            foreach (var plan in _actionPlan.Actions)
                builder.Append($"{plan.Name}({plan.Cost}), ");
            _planText.text = $"Plan: {builder.ToString()}";

            if (_actionPlan != null && _actionPlan.Actions.Count > 0) 
            {
                _navMeshAgent.ResetPath();

                _currentGoal = _actionPlan.AgentGoal;
                Debug.Log($"Goal: {_currentGoal.Name}, plans: {_actionPlan.Actions.Count}");
                _currentAction = _actionPlan.Actions.Pop();
                Debug.Log($"Popped action: {_currentAction.Name}");
                _currentAction.Start();
            }
        }

        if (_actionPlan != null && _currentAction != null)
        {
            _currentAction.Update(Time.deltaTime);
            if (_currentAction.Complete)
            {
                Debug.Log($"{_currentAction.Name} complete");
                _currentAction.Stop();
                _currentAction = null;
                if (_actionPlan.Actions.Count == 0)
                {
                    Debug.Log($"plan complete");
                    _previousGoal = _currentGoal;
                    _currentGoal = null;
                }
            }
        }
    }
    
    private void SetupBeliefs()
    {
        Beliefs = new Dictionary<string, AgentBelief>();
        BeliefFactory factory = new BeliefFactory(this, Beliefs);
        
        factory.AddBelief("Nothing", () => false);
        factory.AddBelief("AgentIdle", () => !_navMeshAgent.hasPath);
        factory.AddBelief("AgentMoving", () => _navMeshAgent.hasPath);
        factory.AddBelief("AgentStaminaLow", () => _sheep.Stat.Stamina < 10);
        factory.AddBelief("AgentStaminaHigh", () => _sheep.Stat.Stamina >= 50);
        factory.AddBelief("AgentHungerLow", () => _sheep.Stat.Hunger < 10);
        factory.AddBelief("AgentHungerHigh", () => _sheep.Stat.Hunger >= 50);
        
        factory.AddLocationBelief("AgentAtBush", 0.1f, _bush.transform);
    }
    
    private void SetupActions()
    {
        Actions = new HashSet<AgentAction>();
        Actions.Add(new AgentAction.Builder("Relax")
            .WithStrategy(new IdleStrategy(_sheep, 5))
            .AddEffect(Beliefs["Nothing"])
            .Build());
        
        Actions.Add(new AgentAction.Builder("WanderAround")
            .WithStrategy(new WanderStrategy(_sheep, _navMeshAgent, 1))
            .AddEffect(Beliefs["AgentMoving"])
            .Build());
        
        Actions.Add(new AgentAction.Builder("MoveToEat")
            .WithStrategy(new MoveStrategy(_sheep, _navMeshAgent, () => _bush.transform.position))
            .AddEffect(Beliefs["AgentAtBush"])
            .Build());

        Actions.Add(new AgentAction.Builder("Eat")
            .WithStrategy(new EatStrategy(_sheep, _bush))
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
        float priority = _currentGoal?.Priority ?? 0;
        HashSet<AgentGoal> goalsToCheck = Goals;

        if (_currentGoal != null)
            goalsToCheck = new HashSet<AgentGoal>(Goals.Where(g => g.Priority > priority));

        ActionPlan potentialPlan = _planner.Plan(this, goalsToCheck, _previousGoal);
        if (potentialPlan != null)
            _actionPlan = potentialPlan;
    }
}
