using System;
using System.Net.Http;
using Azure.Identity;
using MazeWalker.Adapters.Cosmos;
using MazeWalker.Adapters.TvMazeApi;
using MazeWalker.Core;
using MazeWalker.Core.MazeWalkerApi;
using MazeWalker.Core.ScraperState;
using MazeWalker.Core.Scraping;
using MazeWalker.Core.TvMazeApi;
using MazeWalker.FunctionApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MazeWalker.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services
                .AddTransient<Scraper>()
                .AddTransient<IPageScraper, PageScraper>()
                .AddTransient<GetShowsRequestHandler>()
                .AddTransient<IScraperStateRepository, ScrapeStateRepository>()
                .AddTransient<IShowInfoRepository, ShowInfoRepository>();
            builder.Services
                .AddHttpClient<ITvMazeClient, TvMazeClient>()
                .ConfigureHttpClient(client => client.BaseAddress = TvMazeUris.BaseUri)
                .AddPolicyHandler(GetRetryPolicy());
            builder.Services.AddLogging(l => l.AddConsole());
            RegisterOptions(builder.Services);

            builder.Services.AddTransient(GetCosmosDatabase);
        }

        private Database GetCosmosDatabase(IServiceProvider serviceProvider)
        {
            var appConfiguration = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
            var cosmosDatabase = CosmosClientConnector.CreateClient(appConfiguration, new DefaultAzureCredential()).GetAwaiter().GetResult();
            return cosmosDatabase;
        }
        
        private static void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<AppConfiguration>()
                .Configure<IConfiguration>((options, configuration) => {
                    configuration.Bind(options);
                });
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    OnRetry);
        }

        private void OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timeToNextAttempt, int retryAttempt, Context context)
        {
            Console.WriteLine($"Retrying API call, attempt: {retryAttempt}, time to next attempt: {timeToNextAttempt}");
        }
    }
}