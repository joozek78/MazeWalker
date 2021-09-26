using System;

namespace MazeWalker.Adapters.FundaApi
{
    public static class FundaApiUris
    {
        public static Uri BaseUri(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(apiKey));
            }
            return new Uri($"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/{apiKey}/");
        }

        public static Uri Search(int page, string searchTerm) => new Uri(
            $"?type=koop&zo={searchTerm}&pagesize=25&page={page}", 
            UriKind.Relative);
    }
}