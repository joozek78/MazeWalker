using System;
using System.Collections.Generic;
using MazeWalker.Core.Domain;

namespace MazeWalker.Core.TvMazeApi.Models
{
    public class TvMazeGetShowsResponse
    {
        private TvMazeGetShowsResponse()
        {
            NoMoreShows = true;
            Shows = ArraySegment<ShowBasicInfo>.Empty;
        }

        public TvMazeGetShowsResponse(IReadOnlyCollection<ShowBasicInfo> shows)
        {
            NoMoreShows = false;
            Shows = shows;
        }

        public static TvMazeGetShowsResponse CreateNoMoreShows() => new TvMazeGetShowsResponse();
        
        public bool NoMoreShows { get; set; }
        public IReadOnlyCollection<ShowBasicInfo> Shows { get; set; }
    }
}