using System.Collections.Generic;
using MazeWalker.Core.Domain;

namespace MazeWalker.Core.TvMazeApi.Models
{
    public class TvMazeGetCastResponse
    {
        public TvMazeGetCastResponse(IReadOnlyCollection<Person> cast)
        {
            Cast = cast;
        }

        public IReadOnlyCollection<Person> Cast { get; }
    }
}