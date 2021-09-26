using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;
using MazeWalker.Core.FundaApi;

namespace MazeWalker.Core
{
    public class TopPropertiesHandler
    {
        private readonly IFundaApiClient _fundaApiClient;

        public TopPropertiesHandler(IFundaApiClient fundaApiClient)
        {
            _fundaApiClient = fundaApiClient;
        }
        
        public async Task<ApiTopPropertiesResponse> Handle(string searchTerm, int limit)
        {
            var allProperties = await GetAllProperties(searchTerm);
            return new ApiTopPropertiesResponse()
            {
                AgentsResults = allProperties
                    .GroupBy(p=> p.Agent.AgentId)
                    .Select(agentGrouping => new ApiAgentResult()
                {
                    AgentId = agentGrouping.Key,
                    Name = agentGrouping.First().Agent.AgentName,
                    TotalProperties = agentGrouping.Count()
                })
                    .OrderByDescending(result => result.TotalProperties)
                    .Take(limit)
                    .ToList()
            };
        }

        private async Task<IReadOnlyCollection<Property>> GetAllProperties(string searchTerm)
        {
            int page = 1;
            var properties = new List<Property>();
            PropertiesPage pageResult;
            do
            {
                pageResult = await _fundaApiClient.SearchProperties(searchTerm, page);
                properties.AddRange(pageResult.Properties);
                page++;
            } while (pageResult.Properties.Any());

            return properties;
        }

    }
}