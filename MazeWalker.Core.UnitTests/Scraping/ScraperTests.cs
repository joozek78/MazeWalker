using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Scraping;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace MazeWalker.Core.UnitTests.Scraping
{
    public class ScraperTests
    {
        private readonly int _maxPagesPerInvocation = 10;
        private Mock<IPageScraper> _pageScraper;
        private Scraper _scraper;

        [SetUp]
        public void SetUp()
        {
            _pageScraper = new Mock<IPageScraper>();
            _scraper = new Scraper(Options.Create(new AppConfiguration()
                {
                    MaxPagesPerInvocation = _maxPagesPerInvocation
                }),
                Mock.Of<ILogger<Scraper>>(),
                _pageScraper.Object);
        }

        [Test]
        public async Task ShouldScrapeMaxNumberOfPagesIfSuccessful()
        {
            _pageScraper
                .Setup(scraper => scraper.ScrapeNextPage(CancellationToken.None))
                .ReturnsAsync(PageScrapingResult.Success);

            await _scraper.ContinueScraping(CancellationToken.None);
            
            _pageScraper.Verify(scraper => scraper.ScrapeNextPage(CancellationToken.None),
                Times.Exactly(_maxPagesPerInvocation));
        }

        [Test]
        public async Task ShouldStopScrapingWhenNoMoreDataIsAvailable()
        {
            _pageScraper
                .SetupSequence(scraper => scraper.ScrapeNextPage(CancellationToken.None))
                .ReturnsAsync(PageScrapingResult.Success)
                .ReturnsAsync(PageScrapingResult.NoMoreData);

            await _scraper.ContinueScraping(CancellationToken.None);
            
            _pageScraper.Verify(scraper => scraper.ScrapeNextPage(CancellationToken.None),
                Times.Exactly(2));
        }
    }
}