using System;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Scraping;
using Microsoft.Azure.WebJobs;

namespace MazeWalker.FunctionApp
{
    public class ScraperAzureFunction
    {
        private readonly Scraper _scraper;

        public ScraperAzureFunction(Scraper scraper)
        {
            _scraper = scraper;
        }
        
        [FunctionName("ScrapeOnSchedule")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, CancellationToken cancellationToken)
        {
            await _scraper.ContinueScraping(cancellationToken);
        }
    }
}
