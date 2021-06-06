using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MazeWalker.Core.Domain;
using MazeWalker.Core.ScraperState;
using MazeWalker.Core.Scraping;
using MazeWalker.Core.TvMazeApi;
using MazeWalker.Core.TvMazeApi.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MazeWalker.Core.UnitTests.Scraping
{
    public class PageScraperTests
    {
        private Mock<IScraperStateRepository> _scraperStateRepository;
        private Mock<ITvMazeClient> _tvMazeClient;
        private Mock<IShowInfoRepository> _showInfoRepository;
        private PageScraper _pageScraper;
        private readonly List<Person> _getCastResponse = new List<Person>();
        private readonly List<ShowBasicInfo> _getShowsResponse = new List<ShowBasicInfo>();
        private int _currentPageNumber = 2;

        [SetUp]
        public void SetUp()
        {
            _scraperStateRepository = new Mock<IScraperStateRepository>();
            _tvMazeClient = new Mock<ITvMazeClient>();
            _showInfoRepository = new Mock<IShowInfoRepository>();
            _pageScraper = new PageScraper(
                _scraperStateRepository.Object,
                _tvMazeClient.Object,
                _showInfoRepository.Object,
                Mock.Of<ILogger<PageScraper>>()
            );

            _scraperStateRepository
                .Setup(repository => repository.Read())
                .ReturnsAsync(() => new ScraperState.ScraperState(_currentPageNumber));
            _tvMazeClient
                .Setup(client => client.GetShows(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TvMazeGetShowsResponse(_getShowsResponse));
            _tvMazeClient
                .Setup(client => client.GetCast(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TvMazeGetCastResponse(_getCastResponse));
        }

        [Test]
        public async Task ShouldUsePageFromScrapeStateWhenCallingApi()
        {
            _currentPageNumber = 2;

            await _pageScraper.ScrapeNextPage(CancellationToken.None);
            
            _tvMazeClient.Verify(client => client.GetShows(_currentPageNumber, default));
        }

        [Test]
        public async Task ShouldReturnNoMoreShowsWhenNoneAreAvailable()
        {
            _tvMazeClient
                .Setup(client => client.GetShows(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TvMazeGetShowsResponse.CreateNoMoreShows());

            var result = await _pageScraper.ScrapeNextPage(CancellationToken.None);

            result.Should().Be(PageScrapingResult.NoMoreData);
        }
        
        [Test]
        public async Task ShouldPassOnShowIdWhenScrapingCastWhenShowsAreAvailable()
        {
            _getShowsResponse.Add(new ShowBasicInfo(1, "test 1"));
            _getShowsResponse.Add(new ShowBasicInfo(2, "test 2"));

            await _pageScraper.ScrapeNextPage(CancellationToken.None);

            _tvMazeClient.Verify(client => client.GetCast(1, default));
            _tvMazeClient.Verify(client => client.GetCast(2, default));
        }

        [Test]
        public async Task ShouldPersistShowWithCast()
        {
            _getShowsResponse.Add(new ShowBasicInfo(1, "test 1"));
            _getShowsResponse.Add(new ShowBasicInfo(2, "test 2"));
            _getCastResponse.Add(new Person(1, "Arthur Testenson", "2020-01-01"));
            
            await _pageScraper.ScrapeNextPage(CancellationToken.None);

            _showInfoRepository.Verify(r => r.WriteShow(new Show(1, "test 1", _getCastResponse), default));
            _showInfoRepository.Verify(r => r.WriteShow(new Show(2, "test 2", _getCastResponse), default));
        }

        [Test]
        public async Task ShouldPersistScrapingStateAndReturnSuccessWhenThereIsData()
        {
            _currentPageNumber = 2;
            _getShowsResponse.Add(new ShowBasicInfo(1, "test 1"));

            var result = await _pageScraper.ScrapeNextPage(CancellationToken.None);

            result.Should().Be(PageScrapingResult.Success);
            _scraperStateRepository.Verify(repository => repository.Write(new ScraperState.ScraperState(_currentPageNumber + 1)));
        }
    }
}