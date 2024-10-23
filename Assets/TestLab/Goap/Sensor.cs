using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    public event Action OnTargetChanged = delegate { };
    
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _timeInterval = 1f;

    private SphereCollider _detectionRange;
    private GameObject _target;
    private Vector3 _lastKnownPosition;
    private float _elapsedTime = 0f;

    public Vector3 TargetPosition => _target ? _target.transform.position : Vector3.zero;
    public bool IsTargetInRange => TargetPosition != Vector3.zero;

    private void Awake()
    {
        _detectionRange = GetComponent<SphereCollider>();
        _detectionRange.isTrigger = true;
        _detectionRange.radius = _detectionRadius;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _timeInterval)
        {
            UpdateTargetPosition(_target);
            _elapsedTime = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UpdateTargetPosition(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UpdateTargetPosition();
    }

    private void UpdateTargetPosition(GameObject target = null)
    {
        _target = target;
        if (IsTargetInRange && (_lastKnownPosition != TargetPosition || _lastKnownPosition != Vector3.zero))
        {
            _lastKnownPosition = TargetPosition;
            OnTargetChanged?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTargetInRange ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
