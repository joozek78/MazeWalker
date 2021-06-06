using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace MazeWalker.Adapters.Cosmos
{
    public static class CosmosContainers
    {
        public static async Task<Container> EnsureShows(Database database)
        {
            var response = await database.CreateContainerIfNotExistsAsync("Shows", "/id");
            return response.Container;
        }
        
        public static async Task<Container> EnsureScrapeState(Database database)
        {
            var response = await database.CreateContainerIfNotExistsAsync("ScrapeState", "/id");
            return response.Container;
        }
        
        public static readonly string ScrapeStateId = "1";
    }
}