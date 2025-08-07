using System;

public class LevelViewModel
{
    public Action OnLevelUp;

    private LevelModel _model;
    private int _level;

    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            RaiseLevelUp();
        }
    }

    public LevelViewModel(LevelModel model)
    {
        _model = model;
        _level = model.Level;
    }

    public void LevelUp()
    {
        _model.SetLevel(_model.Level + 1);
        Level = _model.Level;
    }

    private void RaiseLevelUp() => OnLevelUp?.Invoke();
}
