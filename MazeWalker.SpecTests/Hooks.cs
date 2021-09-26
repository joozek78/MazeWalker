using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoDi;
using MazeWalker.Core.FundaApi;
using TechTalk.SpecFlow;

namespace MazeWalker.SpecTests
{
    [Binding]
    public sealed class Hooks
    {
        [BeforeScenario]
        public void BeforeScenario(IObjectContainer objectContainer)
        {
            var stubFundaApiClient = new StubFundaApiClient();
            objectContainer.RegisterInstanceAs(stubFundaApiClient);
            objectContainer.RegisterInstanceAs<IFundaApiClient>(stubFundaApiClient);
        }
    }
}