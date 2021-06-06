using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;

namespace MazeWalker.Core
{
    public interface IShowInfoRepository
    {
        Task WriteShow(Show show, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Show>> ListShows(int page, CancellationToken cancellationToken = default);
    }
}