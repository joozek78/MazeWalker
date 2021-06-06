using System.Linq;
using System.Threading.Tasks;
using MazeWalker.Adapters.Cosmos.Models;
using MazeWalker.Core.ScraperState;
using Microsoft.Azure.Cosmos;

namespace MazeWalker.Adapters.Cosmos
{
    public class ScrapeStateRepository : IScraperStateRepository
    {
        private readonly Database _database;

        public ScrapeStateRepository(Database database)
        {
            _database = database;
        }
        
        public async Task<ScraperState> Read()
        {
            var container = await CosmosContainers.EnsureScrapeState(_database);
            var response = container
                .GetItemLinqQueryable<CosmosScraperState>(true)
                .Where(c => c.Id == CosmosContainers.ScrapeStateId)
                .AsEnumerable()
                .FirstOrDefault();
            if (response == null)
            {
                return null;
            }
            return new ScraperState()
            {
                CurrentPageNumber = response.CurrentPageNumber
            };
        }

        public async Task Write(ScraperState scraperState)
        {
            var container = await CosmosContainers.EnsureScrapeState(_database);
            await container.UpsertItemAsync(new CosmosScraperState()
            {
                CurrentPageNumber = scraperState.CurrentPageNumber
            }, new PartitionKey(CosmosContainers.ScrapeStateId));
        }
    }
}