public class LevelModel
{
    public int Level { get; private set; }

    public void SetLevel(int level)
    {
        if (level < 0) return;
        
        Level = level;
    }
}
