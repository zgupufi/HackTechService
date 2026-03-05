namespace HackTech_Service.Models
{
    //Magazine, porți de îmbarcare, toalete (nume, tip, NodeID asociat).
    public class PointOfInterest
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public int NodeId { get; set; }

        // Navigation property
        public virtual Node Node { get; set; }
    }
}
