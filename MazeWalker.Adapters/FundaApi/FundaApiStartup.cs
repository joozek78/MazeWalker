using System;
using System.Net.Http;
using System.Threading.Tasks;
using MazeWalker.Core;
using MazeWalker.Core.FundaApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace MazeWalker.Adapters.FundaApi
{
    public static class FundaApiStartup
    {
        public static IServiceCollection AddFundaApi(this IServiceCollection services)
        {
            services.AddHttpClient<IFundaApiClient, FundaApiClient>()
                .ConfigureHttpClient((sp, client) =>
                {
                    var apiKey = sp.GetRequiredService<IOptions<AppConfiguration>>().Value.FundaApiKey;
                    client.BaseAddress = FundaApiUris.BaseUri(apiKey);
                })
                .AddPolicyHandler(GetPolicy());
            
            return services;
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            var whenFundaIsUnavailable = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized);

            var retryPolicy = whenFundaIsUnavailable
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    OnRetry);
            var throwDomainException = whenFundaIsUnavailable
                .FallbackAsync(ct => Task.FromException<HttpResponseMessage>(new FundaUnavailableException()));
            return Policy.WrapAsync(retryPolicy,
                    throwDomainException);
        }

        private static void OnRetry(DelegateResult<HttpResponseMessage> outcome, TimeSpan timeToNextAttempt, int retryAttempt, Context context)
        {
            Console.WriteLine($"Retrying API call, attempt: {retryAttempt}, time to next attempt: {timeToNextAttempt}");
        }
    }
}