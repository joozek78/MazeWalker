using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;
using MazeWalker.Core.TvMazeApi;
using MazeWalker.Core.TvMazeApi.Models;
using Newtonsoft.Json;

namespace MazeWalker.Adapters.TvMazeApi
{
    public class TvMazeClient: ITvMazeClient
    {
        private readonly HttpClient _httpClient;

        public TvMazeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<TvMazeGetShowsResponse> GetShows(int page, CancellationToken cancellationToken = default)
        {
            var responseMessage = await _httpClient.GetAsync(TvMazeUris.GetShows(page), cancellationToken);
            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return TvMazeGetShowsResponse.CreateNoMoreShows();
            }
            responseMessage.EnsureSuccessStatusCode();
            var asString = await responseMessage.Content.ReadAsStringAsync();
            var tvMazeShows = JsonConvert.DeserializeObject<List<TvMazeShow>>(asString);
            return new TvMazeGetShowsResponse(tvMazeShows.Select(MapToShowBasicInfo).ToList());
        }

        public async Task<TvMazeGetCastResponse> GetCast(int showId, CancellationToken cancellationToken = default)
        {
            var responseMessage = await _httpClient.GetAsync(TvMazeUris.GetCast(showId), cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var asString = await responseMessage.Content.ReadAsStringAsync();
            var tvMazeCast = JsonConvert.DeserializeObject<List<TvMazeCastMember>>(asString);
            return new TvMazeGetCastResponse(tvMazeCast.Select(MapToPerson).ToList());
        }
        
        private static ShowBasicInfo MapToShowBasicInfo(TvMazeShow tvMazeShow) =>
            new ShowBasicInfo(tvMazeShow.Id, tvMazeShow.Name);

        private static Person MapToPerson(TvMazeCastMember castMember) => new Person(castMember.Person.Id,
            castMember.Person.Name, castMember.Person.Birthday);
    }
}