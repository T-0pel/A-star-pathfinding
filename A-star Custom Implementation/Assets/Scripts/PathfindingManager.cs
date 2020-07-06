using Priority_Queue;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathfindingManager : MonoBehaviour
{
    private Grid _grid;

    // Start is called before the first frame update
    void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public List<Node> GetPathWithList(Node startingNode, Node endNode)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var openNodes = new List<Node>();
        var closedNodes = new HashSet<Node>();

        openNodes.Add(startingNode);

        while (openNodes.Count > 0)
        {
            var currentNode = openNodes.OrderBy(n => n.FCost).ThenBy(n => n.HCost).First();
            closedNodes.Add(currentNode);
            openNodes.Remove(currentNode);

            if (currentNode == endNode)
            {
                stopwatch.Stop();
                Debug.Log($"Path found in {stopwatch.ElapsedMilliseconds} ms");
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

    public List<Node> GetPathWithSimplePriorityQueue(Node startingNode, Node endNode)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var openNodes = new SimplePriorityQueue<Node>();
        var closedNodes = new HashSet<Node>();

        openNodes.Enqueue(startingNode, 0);

        while (openNodes.Count > 0)
        {
            var currentNode = openNodes.Dequeue();
            closedNodes.Add(currentNode);

            if (currentNode == endNode)
            {
                stopwatch.Stop();
                Debug.Log($"Path found in {stopwatch.ElapsedMilliseconds} ms");
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

                        if (isNeighbourInOpen)
                        {
                            openNodes.UpdatePriority(neighbour, neighbour.FCost + neighbour.GCost);
                        }
                        else
                        {
                            openNodes.Enqueue(neighbour, neighbour.FCost + neighbour.GCost);
                        }
                    }
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> GetPathWithFastPriorityQueue(Node startingNode, Node endNode)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var openNodes = new FastPriorityQueue<Node>(_grid.GridSizeX * _grid.GridSizeZ);
        var closedNodes = new HashSet<Node>();

        openNodes.Enqueue(startingNode, 0);

        while (openNodes.Count > 0)
        {
            var currentNode = openNodes.Dequeue();
            closedNodes.Add(currentNode);

            if (currentNode == endNode)
            {
                stopwatch.Stop();
                Debug.Log($"Path found in {stopwatch.ElapsedMilliseconds} ms");
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

                        if (isNeighbourInOpen)
                        {
                            openNodes.UpdatePriority(neighbour, neighbour.FCost + neighbour.GCost);
                        }
                        else
                        {
                            openNodes.Enqueue(neighbour, neighbour.FCost + neighbour.GCost);
                        }
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
