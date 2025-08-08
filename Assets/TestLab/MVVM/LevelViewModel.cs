using System;

public class LevelViewModel
{
    public Action OnUpdate;

    private LevelModel _model;

    public string Level { get; private set; }

    public LevelViewModel(LevelModel model)
    {
        _model = model;
        UpdateViewModel();
    }

    public void GainExperience(float amount)
    {
        _model.GainExperience(amount);
        UpdateViewModel();
    }

    private void UpdateViewModel()
    {
        Level = _model.Level.ToString();
        OnUpdate?.Invoke();
    }
}
