using UnityEngine;
using Random = UnityEngine.Random;

public class Key : MonoBehaviour
{
    [SerializeField] private KeyPreset[] presets;
    [SerializeField] private SpriteRenderer keyRenderer;

    private Keys key;
    private Rigidbody2D body2D;

    public Keys KeyValue => key;

    public void RandomKey()
    {
        KeyPreset preset = presets[Random.Range(0, presets.Length)];
        keyRenderer.sprite = preset.KeySprite;
        key = preset.Key;
        body2D = GetComponent<Rigidbody2D>();
    }

    public void WrongPress()
    {
        keyRenderer.color = Color.red;
    }

    public void CorrectPress()
    {
        keyRenderer.color = Color.green;
    }
}
