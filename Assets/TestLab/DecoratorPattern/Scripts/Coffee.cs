using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coffee : MonoBehaviour, ICoffee
{
    [SerializeField] private Liquid liquid;
    [SerializeField][ColorUsage(true, true)] private Color topColor;
    [SerializeField][ColorUsage(true, true)] private Color bottomColor;
    [SerializeField][ColorUsage(true, true)] private Color foamColor;
    [SerializeField] private AudioClip[] hitClips;

    private AudioSource audioSource;
    
    public decimal Cost { get; set; }
    public Dictionary<string, BrewOperation> Ingredients { get; set; } = new();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        liquid.SetFillAmount(10f);
    }

    private void OnCollisionEnter(Collision other)
    {
        AudioClip clip = hitClips[Random.Range(0, hitClips.Length - 1)];
        audioSource.PlayOneShot(clip);
    }

    public void Add()
    {
        
    }

    public void Remove()
    {
        
    }

    public void Brew(List<BrewOperation> brewing)
    {
        liquid.SetFillAmount(0.68f);
        liquid.SetColor(topColor, Liquid.TopColorId);
        liquid.SetColor(bottomColor, Liquid.BottomColorId);
        liquid.SetColor(foamColor, Liquid.FoamColorId);
        
        foreach (BrewOperation brew in brewing)
        {
            for (int i = 0; i < brew.Count; i++)
                brew.Execution(liquid);
        }
    }
}
