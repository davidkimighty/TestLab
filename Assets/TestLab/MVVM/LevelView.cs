using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public Button LevelUpButton;

    private LevelViewModel _viewModel;

    private void Start()
    {
        var viewModel = new LevelViewModel(new LevelModel());
        Setup(viewModel); // testing
    }

    public void Setup(LevelViewModel viewModel)
    {
        _viewModel = viewModel;
        LevelUpButton.onClick.AddListener(() => _viewModel.LevelUp());
        _viewModel.OnLevelUp += UpdateLevel;
    }

    private void UpdateLevel()
    {
        LevelText.text = _viewModel.Level.ToString();
    }
}
