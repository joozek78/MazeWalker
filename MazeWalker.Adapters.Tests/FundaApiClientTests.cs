using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using MazeWalker.Adapters.FundaApi;
using NUnit.Framework;

namespace MazeWalker.Adapters.Tests
{
    public class FundaApiClientTests
    {
         private FundaApiClient _fundaApiClient;

         [SetUp]
         public void SetUp()
         {
             var appConfiguration = new TestConfigProvider().GetConfiguration();
             
             _fundaApiClient = new FundaApiClient(new HttpClient()
             {
                 BaseAddress = FundaApiUris.BaseUri(appConfiguration.FundaApiKey)
             });
         }
         
         [Test]
         public async Task ShouldReturnWellFormedResult()
         {
             var response = await _fundaApiClient.SearchProperties("", 1);
             
             // domain objects are responsible for validating the conversion from raw values
             // if deserialization fails or produces null values an exception will be thrown above
             
             // serialization should be configured to fail if any properties can't be filled, but with
             // default options they will instead get the default value. Hence, we're checking here if
             // int properties have been filled
             response.TotalCount.Should().NotBe(0);
             response.Properties.First().Agent.AgentId.Should().NotBe(0);
         }
    }
}