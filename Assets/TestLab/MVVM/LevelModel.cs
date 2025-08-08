public class LevelModel
{
    private float _experience;
    private int _level;
    public int Level => _level;

    public LevelModel()
    {
        _experience = 0;
        _level = 1;
    }

    public void GainExperience(float amount)
    {
        _experience += amount;
        if (_experience >= 100)
            LevelUp();
    }

    private void LevelUp()
    {
        _level++;
        _experience = 0;
    }
}
