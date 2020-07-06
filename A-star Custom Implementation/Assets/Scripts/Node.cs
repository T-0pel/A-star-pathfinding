using Priority_Queue;
using UnityEngine;

public class Node : FastPriorityQueueNode
{
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public Node Parent { get; set; }
    public int GridX { get; set; }
    public int GridZ { get; set; }
    public Vector3 WorldPosition { get; set; }
    public bool IsTraversable { get; set; } = true;
}
