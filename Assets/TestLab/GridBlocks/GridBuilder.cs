using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    [SerializeField] private float _cellSize;
    [SerializeField] private GameObject _slotPrefab;

    private GameObject[,] _grid;

    private void Start()
    {
        GenerateGrid();
    }
        
    public void GenerateGrid()
    {
        _grid = new GameObject[_row, _column];
            
        for (int i = 0; i < _column; i++)
        {
            for (int j = 0; j < _row; j++)
            {
                Vector3 point = new Vector3(j * _cellSize, i * _cellSize, 0);
                _grid[j, i] = Instantiate(_slotPrefab, point, Quaternion.identity);
                _grid[j, i].transform.parent = transform;
            }
        }
            
        Vector3 gridCenter = new Vector3((_row - 1) * _cellSize, (_column - 1) * _cellSize, 0) / 2f;
        Vector3 camCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        Vector3 centerPoint = camCenter - gridCenter;
        centerPoint.z = 0;
        transform.position = centerPoint;
    }
}
