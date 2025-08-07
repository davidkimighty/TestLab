using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public HealthModel Model;
    public Button TakeDamageButton;

    private void Start()
    {
        TakeDamageButton.onClick.AddListener(HandleTakeDamage);
    }

    private void HandleTakeDamage()
    {
        Model.TakeDamage(10);
    }
}
