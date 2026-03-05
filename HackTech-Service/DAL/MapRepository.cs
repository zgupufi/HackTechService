using HackTech_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace HackTech_Service.DAL
{
    public class MapRepository
    {
        private readonly AirportDbContext _context;

        public MapRepository(AirportDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Node> Nodes, List<Edge> Edges)> GetFullGraphAsync()
        {
            // 1. Luăm toate nodurile (pentru a avea lista completă de puncte)
            var nodes = await _context.Nodes
                .AsNoTracking() 
                .ToListAsync();

            // 2. Luăm toate muchiile și forțăm JOIN cu Nodes 
            // ca să avem coordonatele X, Y direct în obiectele StartNode și EndNode
            var edges = await _context.Edges
                .Include(e => e.StartNode)
                .Include(e => e.EndNode)
                .AsNoTracking()
                .ToListAsync();

            return (nodes, edges);
        }
    }
}