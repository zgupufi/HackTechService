using HackTech_Service.Models;

namespace HackTech_Service.DTOs
{
    // Conține lista de puncte $(x, y)$ pe care frontend-ul trebuie să le deseneze.
    public class RouteResponse
    {
        public List<PathNodeDto> Path { get; set; } = new List<PathNodeDto>();
        public double TotalDistance { get; set; }
        public int EstimatedTimeMinutes { get; set; }
    }
    public class PathNodeDto
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
