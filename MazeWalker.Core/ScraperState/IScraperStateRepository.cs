using System.Threading.Tasks;

namespace MazeWalker.Core.ScraperState
{
    public interface IScraperStateRepository
    {
        Task<ScraperState> Read();
        Task Write(ScraperState scraperState);
    }
}