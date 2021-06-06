using Newtonsoft.Json;

namespace MazeWalker.Adapters.Cosmos.Models
{
    public class CosmosScraperState
    {
        [JsonProperty("id")]
        public string Id => CosmosContainers.ScrapeStateId;

        public int CurrentPageNumber { get; set; }
    }
}