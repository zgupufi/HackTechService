using HackTech_Service.Models;

public class Edge///muchia intre 2 noduri
{
    public int Id { get; set; }
    public int StartNodeId { get; set; }
    public int EndNodeId { get; set; }
    public double Weight { get; set; }
    public bool IsOneWay { get; set; }

    // Proprietăți de navigare (Navigation Properties)
    public virtual Node? StartNode { get; set; }
    public virtual Node? EndNode { get; set; }
}