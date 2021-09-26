using System.Collections.Generic;
using Newtonsoft.Json;

namespace MazeWalker.Adapters.FundaApi.Models
{
    public class FundaApiSearchResult
    {
        [JsonProperty("Objects")]
        public List<FundaApiSearchProperty> Properties { get; set; }
        
        [JsonProperty("TotaalAantalObjecten")]
        public int ResultsCount { get; set; }
    }
}