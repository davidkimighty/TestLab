using UnityEngine;

public class CounterPresenter : MonoBehaviour
{
    private CounterModel _model = new();
    private ICounterView _view;

    private void Start()
    {
        _view = GetComponent<ICounterView>(); // for simplicity
        _view.OnCountClick += HandleCount;
    }

    private void HandleCount()
    {
        _model.Count++;
        _view.UpdateCount(_model.Count);
    }
}
