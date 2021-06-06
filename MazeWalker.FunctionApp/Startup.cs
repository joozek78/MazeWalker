using System;
using System.Net.Http;
using MazeWalker.Adapters.Cosmos;
using MazeWalker.Adapters.TvMazeApi;
using MazeWalker.Core;
using MazeWalker.Core.ScraperState;
using MazeWalker.Core.Scraping;
using MazeWalker.Core.TvMazeApi;
using MazeWalker.FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .AddTransient<IScraper, Scraper>()
                .AddTransient<IScraperStateRepository, ScrapeStateRepository>()
                .AddTransient<IShowInfoRepository, ShowInfoRepository>();
            builder.Services
                .AddHttpClient<ITvMazeClient, TvMazeClient>()
                .AddPolicyHandler(GetRetryPolicy());
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