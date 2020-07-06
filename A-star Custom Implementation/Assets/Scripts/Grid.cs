using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node[,] NodeGrid { get; set; }

    public int GridWorldSizeX;
    public int GridWorldSizeZ;
    public int NodeRadius;
    private int _nodeDiameter;
    private PathfindingManager _pathfindingManager;
    public int GridSizeX { get; set; }
    public int GridSizeZ { get; set; }

    public GameObject StartingObject;
    public GameObject EndObject;
    private List<Node> _path;
    public LayerMask UnwalkableMask;

    void Start()
    {
        _nodeDiameter = NodeRadius * 2;
        GridSizeX = GridWorldSizeX / _nodeDiameter;
        GridSizeZ = GridWorldSizeZ / _nodeDiameter;
        NodeGrid = new Node[GridSizeX, GridSizeZ];
        var bottomLeft = transform.position - Vector3.right * (GridWorldSizeX / 2f) - Vector3.forward * (GridWorldSizeZ / 2f);
        for (var x = 0; x < GridSizeX; x++)
        {
            for (var z = 0; z < GridSizeZ; z++)
            {
                var worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) + Vector3.forward * (z * _nodeDiameter + NodeRadius);

                var isWalkable = !Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask);

                NodeGrid[x, z] = new Node { GridX = x, GridZ = z, WorldPosition = worldPoint, IsTraversable = isWalkable };
            }
        }

        _pathfindingManager = GetComponent<PathfindingManager>();
        _path = _pathfindingManager.GetPathWithFastPriorityQueue(NodeFromWorldPoint(StartingObject.transform.position), NodeFromWorldPoint(EndObject.transform.position));
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        var percentX = (worldPosition.x + GridWorldSizeX / 2) / GridWorldSizeX;
        var percentZ = (worldPosition.z + GridWorldSizeZ / 2) / GridWorldSizeZ;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        var x = Mathf.RoundToInt((GridSizeX - 1) * percentX);
        var z = Mathf.RoundToInt((GridSizeZ - 1) * percentZ);

        return NodeGrid[x, z];
    }

    private void OnDrawGizmos()
    {
        if (NodeGrid != null)
        {
            foreach (var node in NodeGrid)
            {
                if (_path != null && _path.Contains(node))
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.2f));
            }
        }
    }
}
