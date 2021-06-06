using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.ResourceManager.CosmosDB;
using MazeWalker.Core;
using Microsoft.Azure.Cosmos;

namespace MazeWalker.Adapters.Cosmos
{
    public class CosmosClientConnector
    {
        public static async Task<Database> CreateClient(AppConfiguration appAppConfiguration, TokenCredential credential)
        {
            var cosmosDbManagementClient = new CosmosDBManagementClient(appAppConfiguration.SubscriptionId, credential);
            var connectionStringsResponse = await cosmosDbManagementClient.DatabaseAccounts.ListConnectionStringsAsync(
                appAppConfiguration.ResourceGroupName,
                appAppConfiguration.CosmosAccountName);
            var connectionString = connectionStringsResponse.Value.ConnectionStrings.First().ConnectionString;
            var cosmosClient = new CosmosClient(connectionString);
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(appAppConfiguration.CosmosDatabaseName);

            return database;
        }
    }
}