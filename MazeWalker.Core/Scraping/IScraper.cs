using System.Threading;
using System.Threading.Tasks;

namespace MazeWalker.Core.Scraping
{
    public interface IScraper
    {
        Task ContinueScraping(CancellationToken cancellationToken);
    }
}