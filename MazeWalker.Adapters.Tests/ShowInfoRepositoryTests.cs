using System.Threading.Tasks;
using Azure.Identity;
using FluentAssertions;
using MazeWalker.Adapters.Cosmos;
using MazeWalker.Core;
using MazeWalker.Core.Domain;
using MazeWalker.FunctionApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace MazeWalker.Adapters.Tests
{
    public class ShowInfoRepositoryTests
    {
        private ShowInfoRepository _showInfoRepository;
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
            _showInfoRepository = new ShowInfoRepository(_database);
        }
        
        [Test]
        public async Task ShouldPersistShowAndReturnItOnPage()
        {
            var testShow = new Show(1, "Test show",
                new Person[] {new Person(1, "John Doe", "2021-06-06")});
            await _showInfoRepository.WriteShow(testShow);
    
            var shows = await _showInfoRepository.ListShows(0);

            shows.Should().HaveCount(1);
            shows.Should().Contain(testShow);
        }
        
        [Test]
        public async Task ShowOutsideOfPageShouldBeSkipped()
        {
            var testShow = new Show(1, "Test show",
                new Person[] {new Person(1, "John Doe", "2021-06-06")});
            await _showInfoRepository.WriteShow(testShow);

            var shows = await _showInfoRepository.ListShows(1);

            shows.Should().HaveCount(0);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _database.DeleteAsync();
        }
    }
}