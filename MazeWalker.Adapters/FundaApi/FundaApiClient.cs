using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MazeWalker.Adapters.FundaApi.Models;
using MazeWalker.Core.Domain;
using MazeWalker.Core.FundaApi;
using Newtonsoft.Json;

namespace MazeWalker.Adapters.FundaApi
{
    public class FundaApiClient: IFundaApiClient
    {
        private readonly HttpClient _httpClient;

        public FundaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PropertiesPage> SearchProperties(string searchTerm, int pageBase1)
        {
            if (pageBase1 < 1)
            {
                throw new ArgumentException($"{nameof(pageBase1)} can't be lower than 1");
            }

            var responseMessage = await _httpClient.GetAsync(FundaApiUris.Search(pageBase1, searchTerm));
            responseMessage.EnsureSuccessStatusCode();
            var asString = await responseMessage.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<FundaApiSearchResult>(asString);
            return new PropertiesPage(MapToDomain(searchResult.Properties), searchResult.ResultsCount);
        }

        private IReadOnlyCollection<Property> MapToDomain(List<FundaApiSearchProperty> properties)
        {
            return properties
                .Select(p => new Property(
                    p.URL, 
                    p.Address,
                    p.GlobalId,
                    new Agent(p.AgentId, p.AgentName)))
                .ToList();
        }
    }
}