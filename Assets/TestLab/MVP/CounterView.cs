using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour, ICounterView
{
    public event Action OnCount;

    public Button CountButton;
    public TextMeshProUGUI CounterText;

    private void Start()
    {
        CountButton.onClick.AddListener(RaiseCount);
    }

    public void UpdateCount(int count)
    {
        CounterText.text = count.ToString();
    }

    private void RaiseCount()
    {
        OnCount?.Invoke();
    }
}
