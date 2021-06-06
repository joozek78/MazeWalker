using System;

namespace MazeWalker.Adapters.TvMazeApi
{
    public static class TvMazeUris
    {
        public static readonly Uri BaseUri = new Uri("https://api.tvmaze.com/");
        public static Uri GetShows(int page) => new Uri($"/shows?page={page}", UriKind.Relative);
        public static Uri GetCast(int showId) => new Uri($"shows/{showId}/cast", UriKind.Relative);
    }
}