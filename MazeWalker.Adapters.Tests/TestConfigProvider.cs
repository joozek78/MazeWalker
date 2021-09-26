using MazeWalker.Core;
using Microsoft.Extensions.Configuration;

namespace MazeWalker.Adapters.Tests
{
    public class TestConfigProvider
    {
        public AppConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<AppConfiguration>();
        }
    }
}