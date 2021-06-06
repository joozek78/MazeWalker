using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Scraping;
using Microsoft.Azure.WebJobs;

namespace MazeWalker.FunctionApp
{
    public class ScraperAzureFunction
    {
        private readonly IScraper _scraper;

        public ScraperAzureFunction(IScraper scraper)
        {
            _scraper = scraper;
        }
        
        [FunctionName("ScrapeOnSchedule")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer)
        {
            // log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await _scraper.ContinueScraping(CancellationToken.None);
        }
    }
}
