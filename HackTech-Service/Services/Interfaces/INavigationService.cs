using HackTech_Service.Models;

namespace HackTech_Service.Services.Interfaces
{
        public interface INavigationService
        {
            Task<List<Node>> FindShortestPath(int startNodeId, int endNodeId);
        }
    }
