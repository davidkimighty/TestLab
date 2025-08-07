using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterView : MonoBehaviour, ICounterView
{
    public event Action OnCountClick;

    public Button CountButton;
    public TextMeshProUGUI CounterText;

    private void Start()
    {
        CountButton.onClick.AddListener(HandleClick);
    }

    public void UpdateCount(int count)
    {
        CounterText.text = count.ToString();
    }

    private void HandleClick()
    {
        OnCountClick?.Invoke();
    }
}
