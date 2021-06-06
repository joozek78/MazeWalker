using System.Threading.Tasks;
using Azure.Identity;
using FluentAssertions;
using MazeWalker.Adapters.Cosmos;
using MazeWalker.Core;
using MazeWalker.Core.Domain;
using MazeWalker.Core.ScraperState;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace MazeWalker.Adapters.Tests
{
    public class ScrapeStateRepositoryTests
    {
        private ScrapeStateRepository _scrapeStateRepository;
        private Database _database;

        [SetUp]
        public async Task SetUp()
        {
            var appConfiguration = new AppConfiguration();
            new ConfigurationBuilder()
                .AddUserSecrets("c6c35aa3-c081-4a76-b3cb-d51053e966e8")
                .Build()
                .Bind(appConfiguration);
            appConfiguration.CosmosDatabaseName = "MazeWalker.AdapterTests";
            _database = await CosmosClientConnector.CreateClient(appConfiguration, new DefaultAzureCredential(includeInteractiveCredentials: true));
            await _database.DeleteAsync();
            await _database.Client.CreateDatabaseAsync(appConfiguration.CosmosDatabaseName);
            _scrapeStateRepository = new ScrapeStateRepository(_database);
        }
        
        [Test]
        public async Task ShouldPersistStateAndReturnIt()
        {
            var scraperState = new ScraperState()
            {
                CurrentPageNumber = 2
            };

            await _scrapeStateRepository.Write(scraperState);
            var readBack = await _scrapeStateRepository.Read();

            readBack.CurrentPageNumber.Should().Be(2);
        }
        
        [Test]
        public async Task ShouldReturnNullWhenNoStateHasBeenPreviouslyStored()
        {
            var result = await _scrapeStateRepository.Read();
            result.Should().BeNull();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _database.DeleteAsync();
        }

    }
}