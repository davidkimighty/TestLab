using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour, IHealthView
{
    public HealthModel Model;
    public TextMeshProUGUI HealthText;

    private void Start()
    {
        Model.OnHealthChanged += UpdateHealth;

        UpdateHealth(Model.Health);
    }

    public void UpdateHealth(int health)
    {
        HealthText.text = health.ToString();
    }
}
