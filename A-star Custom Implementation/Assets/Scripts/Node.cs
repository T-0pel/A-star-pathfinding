public class Node
{
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public Node Parent { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
}
