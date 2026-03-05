namespace HackTech_Service.DTOs
{
    /// <summary>
    /// Acesta este obiectul pe care frontend-ul îl trimite către tine atunci când utilizatorul vrea să ajungă undeva
    /// </summary>
    public class RouteRequest
    {
        // ID-ul punctului unde se află clientul (extras din scanarea QR)
        public int StartNodeId { get; set; }
        // ID-ul destinației (magazin, poartă, etc.)
        public int EndNodeId { get; set; }
    }
}
