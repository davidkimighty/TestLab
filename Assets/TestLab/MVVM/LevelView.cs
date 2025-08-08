using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour, ILevelView
{
    public event Action OnGainExp;

    public TextMeshProUGUI LevelText;
    public Button GainExpButton;

    private LevelViewModel _viewModel;

    private void OnDestroy()
    {
        if (_viewModel != null)
            _viewModel.OnUpdate -= UpdateLevel;
    }

    public void Setup(LevelViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnUpdate += UpdateLevel;
        UpdateLevel();

        GainExpButton.onClick.AddListener(() => OnGainExp?.Invoke());
    }

    private void UpdateLevel()
    {
        LevelText.text = _viewModel.Level;
    }

}
