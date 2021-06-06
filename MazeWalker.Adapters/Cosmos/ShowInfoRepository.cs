using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Adapters.Cosmos.Models;
using MazeWalker.Core;
using MazeWalker.Core.Domain;
using Microsoft.Azure.Cosmos;

namespace MazeWalker.Adapters.Cosmos
{
    public class ShowInfoRepository : IShowInfoRepository
    {
        private readonly Database _database;

        public ShowInfoRepository(Database database)
        {
            _database = database;
        }
        
        public async Task WriteShow(Show show, CancellationToken cancellationToken = default)
        {
            var shows = await CosmosContainers.EnsureShows(_database);
            var cosmosShow = new CosmosShow()
            {
                IdNumber = show.ShowId,
                Name = show.Name,
                Cast = show.Cast.Select(c => new CosmosPerson()
                {
                    PersonId = c.PersonId,
                    Name = c.Name,
                    Birthday = c.Birthday
                }).ToList()
            };
            await shows.UpsertItemAsync(cosmosShow,
                new PartitionKey(cosmosShow.Id),
                cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<Show>> ListShows(int page, CancellationToken cancellationToken = default)
        {
            var shows = await CosmosContainers.EnsureShows(_database);
            var (minExclusive, maxInclusive) = CosmosShowPages.GetIndexBoundsForPage(page);
            // throw new Exception();
            return shows.GetItemLinqQueryable<CosmosShow>(true)
                .Where(show => show.IdNumber > minExclusive && show.IdNumber <= maxInclusive)
                .AsEnumerable()
                .Select(MapToDomain)
                .ToList();
        }

        private Show MapToDomain(CosmosShow cosmosShow)
        {
            return new Show(cosmosShow.IdNumber, 
                cosmosShow.Name,
                cosmosShow.Cast.Select(c => new Person(c.PersonId, c.Name, c.Birthday)).ToList());
        }
    }
}