using UnityEngine;

public class HealthController : MonoBehaviour
{
    private IHealthView _view;
    private HealthModel _model;

    private void Start()
    {
        _model = new HealthModel(100);
        _view = GetComponent<IHealthView>();
        _view.Initialize(_model);

        _view.OnTakeDamage += HandleTakeDamage;
    }

    private void HandleTakeDamage()
    {
        _model.TakeDamage(10);
    }
}
