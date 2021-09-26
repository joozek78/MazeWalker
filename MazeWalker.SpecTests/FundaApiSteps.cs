using System;
using System.Linq;
using MazeWalker.Core.Domain;
using TechTalk.SpecFlow;

namespace MazeWalker.SpecTests
{
    [Binding]
    public class FundaApiSteps
    {
        private readonly StubFundaApiClient _fundaApiClient;

        public FundaApiSteps(StubFundaApiClient fundaApiClient)
        {
            _fundaApiClient = fundaApiClient;
        }
        
        [Given(@"there are following properties in funda")]
        public void GivenThereAreFollowingPropertiesInFunda(Table table)
        {
            _fundaApiClient.SetOnSinglePage(table.Rows.Select(MapToProperty).ToList());
        }

        private Property MapToProperty(TableRow row)
        {
            var address = row["Address"];
            return new Property(address, address, address, new Agent(
                Convert.ToInt32(row["AgentId"]), row["AgentName"]));
        }

        [Given(@"there are following properties in funda on page (.*)")]
        public void GivenThereAreFollowingPropertiesInFundaOnPage(int page, Table table)
        {
            _fundaApiClient.SetPage(page, table.Rows.Select(MapToProperty).ToList());
        }
    }
}