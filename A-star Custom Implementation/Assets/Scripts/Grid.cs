using UnityEngine;

public class Grid : MonoBehaviour
{
    private Node[,] _grid;

    public int GridWorldSizeX;
    public int GridWorldSizeZ;
    public int NodeRadius;
    private int _nodeDiameter;
    private int _gridSizeX;
    private int _gridSizeZ;

    // Start is called before the first frame update
    void Start()
    {
        _nodeDiameter = NodeRadius * 2;
        _gridSizeX = GridWorldSizeX / _nodeDiameter;
        _gridSizeZ = GridWorldSizeZ / _nodeDiameter;
        _grid = new Node[_gridSizeX, _gridSizeZ];
        var bottomLeft = transform.position - Vector3.right * (GridWorldSizeX / 2f) - Vector3.forward * (GridWorldSizeZ / 2f);
        for (var x = 0; x < _gridSizeX; x++)
        {
            for (var z = 0; z < _gridSizeZ; z++)
            {
                var worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.forward * (z * _nodeDiameter + NodeRadius);
                _grid[x, z] = new Node { GridX = x, GridZ = z, WorldPosition = worldPoint };
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_grid != null)
        {
            foreach (var node in _grid)
            {
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
            }
        }
    }
}
