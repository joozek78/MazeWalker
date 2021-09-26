using Newtonsoft.Json;

namespace MazeWalker.Adapters.FundaApi.Models
{
    public class FundaApiSearchProperty
    {
        public string URL { get; set; }
        
        [JsonProperty("MakelaarId")]
        public int AgentId { get; set; }
        
        [JsonProperty("MakelaarNaam")]
        public string AgentName { get; set; }
        
        [JsonProperty("Adres")]
        public string Address { get; set; }
        
        [JsonProperty("GlobalId")]
        public string GlobalId { get; set; }
    }
}