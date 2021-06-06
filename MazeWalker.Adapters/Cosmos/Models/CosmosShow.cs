using System.Collections.Generic;
using Newtonsoft.Json;

namespace MazeWalker.Adapters.Cosmos.Models
{
    public class CosmosShow
    {
        [JsonProperty("id")] 
        public string Id => IdNumber.ToString();

        public int IdNumber { get; set; }
        public string Name { get; set; }
        public IReadOnlyCollection<CosmosPerson> Cast { get; set; }
    }
}