using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    private Grid _grid;

    // Start is called before the first frame update
    void Start()
    {
        _grid = GetComponent<Grid>();
    }

    public List<Node> GetPath(Node startingNode, Node endNode)
    {
        var openNodes = new List<Node>();
        var closedNodes = new List<Node>();

        openNodes.Add(startingNode);

        while (openNodes.Count > 0)
        {
            var currentNode = openNodes.OrderBy(n => n.FCost).ThenBy(n => n.HCost).First();
            closedNodes.Add(currentNode);
            openNodes.Remove(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(currentNode);
            }

            for (var x = -1; x <= 1; x++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;

                    var gridX = currentNode.GridX + x;
                    var gridZ = currentNode.GridZ + z;
                    if (gridX < 0 || gridX >= _grid.GridSizeX ||
                        gridZ < 0 || gridZ >= _grid.GridSizeZ)
                    {
                        continue;
                    }

                    var neighbour = _grid.NodeGrid[gridX, gridZ];

                    if (!neighbour.IsTraversable || closedNodes.Contains(neighbour)) continue;

                    var gCost = GetDistance(neighbour, startingNode);
                    var hCost = GetDistance(neighbour, endNode);

                    var movementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    var isNeighbourInOpen = openNodes.Contains(neighbour);
                    if (movementCostToNeighbour < neighbour.GCost || neighbour.GCost == 0)
                    {
                        neighbour.GCost = movementCostToNeighbour;
                        neighbour.HCost = hCost;
                        neighbour.Parent = currentNode;

                        if (!isNeighbourInOpen) openNodes.Add(neighbour);
                    }
                }
            }
        }

        return new List<Node>();
    }

    private static int GetDistance(Node nodeA, Node nodeB)
    {
        var distanceX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        var distanceZ = Mathf.Abs(nodeA.GridZ - nodeB.GridZ);

        if (distanceX > distanceZ)
        {
            return 14 * distanceZ + 10 * (distanceX - distanceZ);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceZ - distanceX);
        }
    }

    private static List<Node> RetracePath(Node finalNode)
    {
        var pathNodes = new List<Node>();
        var currentNode = finalNode;
        while (currentNode.Parent != null)
        {
            pathNodes.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        pathNodes.Reverse();
        return pathNodes;
    }
}
