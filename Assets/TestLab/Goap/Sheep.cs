using System;
using TMPro;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public enum State
    {
        Idle,
        Bounce,
        Eat
    }
    
    [SerializeField] private Status _status;
    [SerializeField] private float _hungerDecreaseFactor = 0.05f;
    [SerializeField] private float _staminaIncreaseFactor = 0.05f;
    [SerializeField] private float _staminaDecreaseFactor = 0.1f;
    [SerializeField] private float _statUpdateInterval = 1f;
    [Range(0, 100f)] [SerializeField] private float _eatAmount = 10f;
    [SerializeField] private float _eatInterval = 2f;
    [SerializeField] private TMP_Text _hungerText;
    [SerializeField] private TMP_Text _staminaText;
    
    private Animator _animator;
    private float _statElapsedTime;
    private float _eatElapsedTime;

    public Status Stat => _status;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _statElapsedTime += Time.deltaTime;
        if (_statElapsedTime > _statUpdateInterval)
        {
            _status.Hunger = Mathf.Clamp(_status.Hunger - 100f * _hungerDecreaseFactor, 0, 100f);
            _statElapsedTime = 0f;
        }

        _hungerText.text = $"Hunger: {_status.Hunger}";
        _staminaText.text = $"Stamina: {_status.Stamina}";
    }

    public void IdleAnimation()
    {
        _animator.SetTrigger(State.Idle.ToString());
    }
    
    public void BounceAnimation()
    {
        _animator.SetTrigger(State.Bounce.ToString());
    }

    public void EatAnimation()
    {
        _animator.SetTrigger(State.Eat.ToString());
        _eatElapsedTime = Mathf.Infinity;
    }

    public void Resting()
    {
        float stamina = _status.Stamina + 100f * _staminaIncreaseFactor;
        _status.Stamina = Mathf.Clamp(stamina, 0, 100f);
    }
    
    public void Moving()
    {
        float stamina = _status.Stamina - 100f * _staminaDecreaseFactor;
        _status.Stamina = Mathf.Clamp(stamina, 0, 100f);
    }

    public bool Eating(float deltaTime, Bush bush)
    {
        if (!bush.CanEat()) return false;
        
        _eatElapsedTime += deltaTime;
        if (_eatElapsedTime > _eatInterval)
        {
            _eatElapsedTime = 0f;
            float amount = bush.Eat(_eatAmount);
            _status.Stamina = Mathf.Clamp(_status.Stamina + amount, 0, 100f);
            _status.Hunger = Mathf.Clamp(_status.Hunger + amount, 0, 100f);
        }
        return true;
    }

    [Serializable]
    public struct Status
    {
        public float Hunger;
        public float Stamina;
    }
}
