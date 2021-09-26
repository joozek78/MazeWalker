using System;
using System.Net.Http;
using MazeWalker.Adapters.FundaApi;
using MazeWalker.Core;
using MazeWalker.Core.FundaApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace MazeWalker.AspNet
{
    public static class FundaApiStartup
    {
        public static IServiceCollection AddFundaApi(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient<IFundaApiClient, FundaApiClient>()
                .ConfigureHttpClient((sp, client) =>
                {
                    var apiKey = sp.GetRequiredService<IOptions<AppConfiguration>>().Value.FundaApiKey;
                    client.BaseAddress = FundaApiUris.BaseUri(apiKey);
                })
                .AddPolicyHandler(GetRetryPolicy());
            
            return services;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    OnRetry);
        }

        private static void OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timeToNextAttempt, int retryAttempt, Context context)
        {
            Console.WriteLine($"Retrying API call, attempt: {retryAttempt}, time to next attempt: {timeToNextAttempt}");
        }
    }
}