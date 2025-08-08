using System;

public interface ILevelView
{
    event Action OnGainExp;

    void Setup(LevelViewModel viewModel);
}
