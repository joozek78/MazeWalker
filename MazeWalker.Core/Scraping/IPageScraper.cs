using System.Threading;
using System.Threading.Tasks;

namespace MazeWalker.Core.Scraping
{
    public interface IPageScraper
    {
        Task<PageScrapingResult> ScrapeNextPage(CancellationToken cancellationToken);
    }
}