using UnityEngine;

public class Grid : MonoBehaviour
{
    private Node[,] _grid;

    public int GridSizeX;
    public int GridSizeZ;
    public int NodeSize;

    // Start is called before the first frame update
    void Start()
    {
        _grid = new Node[GridSizeX, GridSizeZ];
        var bottomLeft = new Vector3(transform.position - Vector3.right * (GridSizeX / 2), 0, );
        for (var x = -GridSizeX; x < GridSizeX; x++)
        {
            for (var z = -GridSizeZ; z < GridSizeZ; z++)
            {
                _grid[x, z] = new Node { X = x, Z = z };
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_grid != null)
        {
            foreach (var node in _grid)
            {
                Gizmos.DrawCube(new Vector3(node.X, 0.1f, node.Z), Vector3.one);
            }
        }
    }
}
