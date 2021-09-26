using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;
using MazeWalker.Core.FundaApi;

namespace MazeWalker.SpecTests
{
    public class StubFundaApiClient : IFundaApiClient
    {
        private const int MaxPageNumber = 10;
        private IReadOnlyCollection<Property>[] _pages = new IReadOnlyCollection<Property>[MaxPageNumber];
        public Task<PropertiesPage> SearchProperties(string searchTerm, int pageBase1,
            CancellationToken cancellationToken)
        {
            if (pageBase1 > _pages.Length)
            {
                return Task.FromResult(new PropertiesPage(Array.Empty<Property>(), TotalCount));
            }

            var page = _pages[pageBase1 - 1] ?? Array.Empty<Property>();
            return Task.FromResult(new PropertiesPage(page, TotalCount));
        }

        private int TotalCount => _pages.Sum(p => p?.Count ?? 0);

        public void SetOnSinglePage(IReadOnlyCollection<Property> properties)
        {
            _pages = new[] {properties};
        }

        public void SetPage(int page, IReadOnlyCollection<Property> properties)
        {
            _pages[page - 1] = properties;
        }
    }
}