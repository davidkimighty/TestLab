using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IHealthView
{
    public event Action OnTakeDamage;

    public Button TakeDamageButton;
    public TextMeshProUGUI HealthText;

    private HealthModel _healthModel;

    private void Start()
    {
        TakeDamageButton.onClick.AddListener(RaiseTakeDamage);
    }

    private void OnDestroy()
    {
        if (_healthModel != null)
            _healthModel.OnHealthChanged -= UpdateHealth;
    }

    public void Initialize(HealthModel model)
    {
        _healthModel = model;
        _healthModel.OnHealthChanged += UpdateHealth;
        UpdateHealth(_healthModel.Health);
    }

    private void UpdateHealth(int health)
    {
        HealthText.text = health.ToString();
    }

    private void RaiseTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }
}
