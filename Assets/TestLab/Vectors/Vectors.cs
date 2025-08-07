using UnityEngine;

public class Vectors : MonoBehaviour
{
    [SerializeField] private Transform _a;
    [SerializeField] private Transform _axis;

    private void Start()
    {
        Vector3 axis = _axis.right.normalized;
        Vector3 projection = Vector3.Project(_a.position, axis);
        float dist = Vector3.Distance(_a.position, projection);
        _a.position = projection;
        Debug.Log($"Distance: {dist}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_axis.position, _axis.right * 100f);
        Gizmos.DrawRay(_axis.position, -_axis.right * 100f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_axis.position, _axis.up * 100f);
        Gizmos.DrawRay(_axis.position, -_axis.up * 100f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_axis.position, _axis.forward * 100f);
        Gizmos.DrawRay(_axis.position, -_axis.forward * 100f);
    }
}
