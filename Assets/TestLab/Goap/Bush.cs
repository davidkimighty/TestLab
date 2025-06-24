using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private List<State> _states;
    [SerializeField] private float _growth = 100f;
    [SerializeField] private float _growthValue = 0.1f;
    [SerializeField] private float _growthInterval = 1f;
    
    private float _elapsedTime;

    private void Awake()
    {
        _states = _states.OrderBy(s => s.Threshold).ToList();
        UpdateVisuals();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _growthInterval)
        {
            _elapsedTime = 0f;
            _growth = Mathf.Clamp(_growth + _growthValue, 0f, 100f);
            UpdateVisuals();
        }
    }

    public bool CanEat() => _growth > _states[0].Threshold;

    public float Eat(float eatAmount)
    {
        float g = _growth - eatAmount;
        float amount = g < 0 ? _growth : g;
        _growth = Mathf.Clamp(_growth - amount, 0f, 100f);
        return amount;
    }

    private void UpdateVisuals()
    {
        foreach (State state in _states)
        {
            if (state.Threshold < _growth) continue;
            
            _renderer.sprite = state.Sprite;
            break;
        }
    }
    
    [Serializable]
    public struct State
    {
        public Sprite Sprite;
        [Range(0, 100)] public int Threshold;
    }
}
