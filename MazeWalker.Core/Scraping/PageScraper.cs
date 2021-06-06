using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;
using MazeWalker.Core.ScraperState;
using MazeWalker.Core.TvMazeApi;
using MazeWalker.Core.TvMazeApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MazeWalker.Core.Scraping
{
    public class PageScraper: IPageScraper
    {
        private readonly IScraperStateRepository _scraperStateRepository;
        private readonly ITvMazeClient _tvMazeClient;
        private readonly IShowInfoRepository _showInfoRepository;
        private readonly ILogger<PageScraper> _logger;

        public PageScraper(
            IScraperStateRepository scraperStateRepository,
            ITvMazeClient tvMazeClient,
            IShowInfoRepository showInfoRepository,
            ILogger<PageScraper> logger)
        {
            _scraperStateRepository = scraperStateRepository;
            _tvMazeClient = tvMazeClient;
            _showInfoRepository = showInfoRepository;
            _logger = logger;
        }
        
        public async Task<PageScrapingResult> ScrapeNextPage(CancellationToken cancellationToken)
        {
            var scraperState = await _scraperStateRepository.Read();

            _logger.LogInformation("Scraping shows from page {CurrentPageNumber}", scraperState.CurrentPageNumber);
            var getShowsResponse = await _tvMazeClient.GetShows(scraperState.CurrentPageNumber, cancellationToken);
            if (getShowsResponse.NoMoreShows)
            {
                _logger.LogInformation("No more data");
                return PageScrapingResult.NoMoreData;
            }

            await ScrapeDetailsAndPersist(getShowsResponse, cancellationToken);
            _logger.LogInformation("Scraped page {CurrentPageNumber} successfully", scraperState.CurrentPageNumber);
            await UpdateAndPersistScraperState(scraperState);
            return PageScrapingResult.Success;
        }

        private async Task ScrapeDetailsAndPersist(TvMazeGetShowsResponse getShowsResponse, CancellationToken cancellationToken)
        {
            var basicShows = getShowsResponse.Shows;
            _logger.LogInformation("Will scrape cast for {numberOfShows} shows", basicShows.Count);
            await Task.WhenAll(basicShows.Select(info => ScrapeCastAndSave(info, cancellationToken)));
        }

        private async Task ScrapeCastAndSave(ShowBasicInfo showBasicInfo, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scraping cast for show '{showTitle}' with id {showId}", showBasicInfo.Name, showBasicInfo.ShowId);
            var getCastResponse = await _tvMazeClient.GetCast(showBasicInfo.ShowId, cancellationToken);
            var show = new Show(showBasicInfo, getCastResponse.Cast);

            await _showInfoRepository.WriteShow(show, cancellationToken);
            _logger.LogInformation("Scraped and persisted cast for show '{showTitle}' with id {showId}", showBasicInfo.Name, showBasicInfo.ShowId);
        }

        private async Task UpdateAndPersistScraperState(ScraperState.ScraperState scraperState)
        {
            await _scraperStateRepository.Write(scraperState.WithNextPage());
        }
    }
}