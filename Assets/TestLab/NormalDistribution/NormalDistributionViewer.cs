using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class NormalDistributionViewer : MonoBehaviour
{
    [Serializable]
    public struct DistributionParameters
    {
        public float Mean;
        public float StandardDeviation;
        public float MinValue;
        public float MaxValue;
    }
    
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _linePoints = 1000;
    [SerializeField] private float _graphScale = 1000f;
    [SerializeField] private DistributionParameters _distributionParam;

    [SerializeField] private TMP_Text _meanText;
    [SerializeField] private TMP_Text _sdText;
    [SerializeField] private TMP_Text _maxText;
    [SerializeField] private TMP_Text _minText;
    [SerializeField] private TMP_Text _ndText;

    private void Start()
    {
        GenerateGraph();
        UpdateUI();
    }

    private void GenerateGraph()
    {
        _line.positionCount = _linePoints;
        float range = _distributionParam.MaxValue - _distributionParam.MinValue;

        float maxY = 0f;
        for (int i = 0; i < _linePoints; i++)
        {
            float x = i * range / (_linePoints - 1) + _distributionParam.MinValue;
            float y = NormalDistributionPDF(x, _distributionParam);
            y *= _graphScale;
            maxY = Math.Max(maxY, y);
            _line.SetPosition(i, new Vector3(x, y, 0f));
        }

        Vector3 pos = Camera.main.transform.position;
        Vector3 targetPos = _line.GetPosition(Mathf.RoundToInt(_linePoints * 0.5f)); 
        pos.x = targetPos.x;
        pos.y = maxY / 2f;
        Camera.main.transform.position = pos;
    }

    private void UpdateUI()
    {
        _meanText.text = $"Mean - {_distributionParam.Mean}";
        _sdText.text = $"Standard Deviation - {_distributionParam.StandardDeviation}";
        _maxText.text = $"Max - {_distributionParam.MaxValue}";
        _minText.text = $"Min - {_distributionParam.MinValue}";
        _ndText.text = $"Normal Distribution - {GenerateNormalDistribution(_distributionParam):0.00}";
    }

    private float GenerateNormalDistribution(DistributionParameters param)
    {
        float r1 = 1f - Random.value;
        float r2 = 1f - Random.value;
        float normalRandom = Mathf.Sqrt(-2f * Mathf.Log(r1)) * Mathf.Cos(2f * Mathf.PI * r2);
            
        float scaledShifted = param.Mean + param.StandardDeviation * normalRandom;
        float result = Mathf.Clamp(scaledShifted, param.MinValue, param.MaxValue);
        return result;
    }
        
    private float NormalDistributionPDF(float x, DistributionParameters parameters)
    {
        float exponent = -0.5f * Mathf.Pow((x - parameters.Mean) / parameters.StandardDeviation, 2f);
        return Mathf.Exp(exponent) / (parameters.StandardDeviation * Mathf.Sqrt(2f * Mathf.PI));
    }

    private void OnValidate()
    {
        GenerateGraph();
        UpdateUI();
    }
}
