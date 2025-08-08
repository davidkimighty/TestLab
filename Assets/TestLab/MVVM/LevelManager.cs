using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float _expGain = 30f;

    private LevelModel _model;
    private LevelViewModel _viewModel;
    private ILevelView _view;

    private void Start()
    {
        _model = new LevelModel();
        _viewModel = new LevelViewModel(_model);

        _view = GetComponent<ILevelView>();
        _view.Setup(_viewModel);
        _view.OnGainExp += HandleGainExp;
    }

    private void HandleGainExp()
    {
        _viewModel.GainExperience(_expGain);
    }
}
