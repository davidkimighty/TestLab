using UnityEngine;

public class CounterPresenter : MonoBehaviour
{
    private CounterModel _model;
    private ICounterView _view;

    private void Start()
    {
        _model = new CounterModel(0);
        _view = GetComponent<ICounterView>(); // for simplicity
        _view.OnCount += HandleCount;
    }

    private void HandleCount()
    {
        _model.CountUp();
        _view.UpdateCount(_model.Count);
    }
}
