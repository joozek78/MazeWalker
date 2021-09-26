using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MazeWalker.Contract;
using MazeWalker.Core;
using TechTalk.SpecFlow;

namespace MazeWalker.SpecTests
{
    [Binding]
    public class TopPropertiesApiSteps
    {
        private readonly TopPropertiesHandler _handler;
        private ApiTopPropertiesResponse _topProperties;
        private int _resultsLimit = 10;

        public TopPropertiesApiSteps(TopPropertiesHandler handler)
        {
            _handler = handler;
        }
        
        [When(@"top agents are determined")]
        public async Task WhenTopAgentsAreDetermined()
        {
            _topProperties = await _handler.Handle("", _resultsLimit);
        }

        [Then(@"the top agents are the following")]
        public void ThenTheTopAgentsAreTheFollowing(Table table)
        {
            var expectation = table.Rows.Select(r => new ApiAgentResult()
            {
                AgentId = Convert.ToInt32(r["AgentId"]),
                Name = r["AgentName"],
                TotalProperties = Convert.ToInt32(r["TotalProperties"])
            }).ToList();
            _topProperties.AgentsResults.Should().BeEquivalentTo(expectation, options => options.WithStrictOrdering());
        }
    }
}