using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.TvMazeApi.Models;

namespace MazeWalker.Core.TvMazeApi
{
    public interface ITvMazeClient
    {
        Task<TvMazeGetShowsResponse> GetShows(int page, CancellationToken cancellationToken = default);
        Task<TvMazeGetCastResponse> GetCast(int showId, CancellationToken cancellationToken = default);
    }
}