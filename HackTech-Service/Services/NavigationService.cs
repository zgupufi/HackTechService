using HackTech_Service.DAL;
using HackTech_Service.Models;
using HackTech_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackTech_Service.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MapRepository _mapRepository;

        public NavigationService(MapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<List<Node>> FindShortestPath(int startNodeId, int endNodeId)
        {
            var (nodes, edges) = await _mapRepository.GetFullGraphAsync();

            if (!nodes.Any() || !edges.Any())
                throw new Exception("Harta nu este populata!");

            var nodeMap = nodes.ToDictionary(n => n.Id);
            var adjacencyList = BuildAdjacencyList(edges);

            if (!nodeMap.ContainsKey(startNodeId) || !nodeMap.ContainsKey(endNodeId))
                return new List<Node>();

            // Rulăm calculul greu pe un thread separat pentru a nu bloca procesorul
            return await Task.Run(() => SolveAStar(nodeMap, adjacencyList, startNodeId, endNodeId));
        }

        // --- METODE PRIVATE (MOTOARELE A*) ---

        private List<Node> SolveAStar(Dictionary<int, Node> nodeMap, Dictionary<int, List<Edge>> adj, int startId, int endId)
        {
            var startNode = nodeMap[startId];
            var endNode = nodeMap[endId];

            var openSet = new List<Node> { startNode };
            var cameFrom = new Dictionary<int, int>(); // Reconstrucție drum: ID_Copil -> ID_Părinte

            // gScore: costul minim de la start la nodul curent
            var gScore = nodeMap.Keys.ToDictionary(id => id, id => double.MaxValue);
            gScore[startId] = 0;

            // fScore: gScore + distanța estimată până la final (Euristică)
            var fScore = nodeMap.Keys.ToDictionary(id => id, id => double.MaxValue);
            fScore[startId] = Heuristic(startNode, endNode);

            while (openSet.Any())
            {
                // Luăm nodul din openSet cu cel mai mic fScore (cel mai promițător)
                var current = openSet.OrderBy(n => fScore[n.Id]).First();

                if (current.Id == endId)
                    return ReconstructPath(cameFrom, nodeMap, endId);

                openSet.Remove(current);

                if (!adj.ContainsKey(current.Id)) continue;

                foreach (var edge in adj[current.Id])
                {
                    var neighbor = nodeMap[edge.EndNodeId];
                    double tentativeGScore = gScore[current.Id] + edge.Weight;

                    if (tentativeGScore < gScore[neighbor.Id])
                    {
                        // Am găsit un drum mai bun către acest vecin!
                        cameFrom[neighbor.Id] = current.Id;
                        gScore[neighbor.Id] = tentativeGScore;
                        fScore[neighbor.Id] = gScore[neighbor.Id] + Heuristic(neighbor, endNode);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return new List<Node>(); // Nu s-a găsit niciun drum
        }

        // Calculează distanța în linie dreaptă între două puncte (Pitagora)
        private double Heuristic(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        // Mergem înapoi prin dicționarul cameFrom pentru a crea lista finală de pași
        private List<Node> ReconstructPath(Dictionary<int, int> cameFrom, Dictionary<int, Node> nodeMap, int currentId)
        {
            var path = new List<Node> { nodeMap[currentId] };
            while (cameFrom.ContainsKey(currentId))
            {
                currentId = cameFrom[currentId];
                path.Insert(0, nodeMap[currentId]);
            }
            return path;
        }

        // Grupează muchiile pentru acces instant și gestionează drumurile cu dublu sens
        private Dictionary<int, List<Edge>> BuildAdjacencyList(List<Edge> edges)
        {
            var adj = new Dictionary<int, List<Edge>>();
            foreach (var edge in edges)
            {
                if (!adj.ContainsKey(edge.StartNodeId)) adj[edge.StartNodeId] = new List<Edge>();
                adj[edge.StartNodeId].Add(edge);

                // Dacă NU este sens unic, creăm o muchie virtuală inversă
                if (!edge.IsOneWay)
                {
                    if (!adj.ContainsKey(edge.EndNodeId)) adj[edge.EndNodeId] = new List<Edge>();
                    adj[edge.EndNodeId].Add(new Edge
                    {
                        StartNodeId = edge.EndNodeId,
                        EndNodeId = edge.StartNodeId,
                        Weight = edge.Weight
                    });
                }
            }
            return adj;
        }
    }
}