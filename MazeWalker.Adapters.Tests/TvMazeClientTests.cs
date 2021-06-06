using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using MazeWalker.Adapters.TvMazeApi;
using MazeWalker.Core.Domain;
using NUnit.Framework;

namespace MazeWalker.Adapters.Tests
{
    public class TvMazeClientTests
    {
        private TvMazeClient _tvMazeClient;

        [SetUp]
        public void SetUp()
        {
            _tvMazeClient = new TvMazeClient(new HttpClient()
            {
                BaseAddress = TvMazeUris.BaseUri
            });
        }
        [Test]
        public async Task ShouldReturnWellKnownShowFromFirstPage()
        {
            var showsResponse = await _tvMazeClient.GetShows(0);

            showsResponse.NoMoreShows.Should().BeFalse();
            showsResponse.Shows.Should().Contain(new ShowBasicInfo(4, "Arrow"));
        }

        [Test]
        public async Task ShouldReturnNoMoreShowsOnAbsurdlyHighPage()
        {
            var showsResponse = await _tvMazeClient.GetShows(10000);

            showsResponse.NoMoreShows.Should().BeTrue();
            showsResponse.Shows.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldReturnWellKnownCastFromShow()
        {
            var castResponse = await _tvMazeClient.GetCast(4);

            castResponse.Cast.Should().HaveCount(25);
            castResponse.Cast.Should().Contain(new Person(274, "David Ramsey", "1971-11-17"));
        }
        
        [Test]
        public void ShouldThrowWhenTryingToGetCastForNonexistentShow()
        {
            _tvMazeClient.Invoking(c => c.GetCast(400000))
                .Should().Throw<HttpRequestException>()
                .And.Message.Should().Contain("404");
        }
    }
}