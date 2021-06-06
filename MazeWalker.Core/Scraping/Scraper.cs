using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MazeWalker.Core.Scraping
{
    public class Scraper
    {
        private readonly ILogger<Scraper> _logger;
        private readonly IPageScraper _pageScraper;
        private readonly AppConfiguration _appConfiguration;

        public Scraper(IOptions<AppConfiguration> appConfiguration,
            ILogger<Scraper> logger,
            IPageScraper pageScraper)
        {
            _logger = logger;
            _pageScraper = pageScraper;
            _appConfiguration = appConfiguration.Value;
        }
        
        public async Task ContinueScraping(CancellationToken cancellationToken)
        {
            var numberOfPagesToScrape = _appConfiguration.MaxPagesPerInvocation;
            _logger.LogInformation("Will scrape at most {numberOfPagesToScrape} pages", numberOfPagesToScrape);
            for (int i = 0; i < numberOfPagesToScrape; i++)
            {
                var pageScrapingResult = await _pageScraper.ScrapeNextPage(cancellationToken);
                if (pageScrapingResult == PageScrapingResult.NoMoreData)
                {
                    _logger.LogInformation("No more shows found. Stopping scraping");
                    return;
                }
            }
            _logger.LogInformation("Scraped max number of pages per invocation. Stopping scraping");
        }

        
    }
}