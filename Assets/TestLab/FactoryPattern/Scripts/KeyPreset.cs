using UnityEngine;

public enum Keys
{
    None,
    Up, Down, Left, Right
}

[CreateAssetMenu(fileName = "KeyPreset", menuName = "TestLab/FactoryPattern/KeyPreset")]
public class KeyPreset : ScriptableObject
{
    public Keys Key;
    public Sprite KeySprite;
}
