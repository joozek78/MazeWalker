using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.Domain;
using MazeWalker.Core.MazeWalkerApi.Models;

namespace MazeWalker.Core.MazeWalkerApi
{
    public class GetShowsRequestHandler
    {
        private readonly IShowInfoRepository _showInfoRepository;

        public GetShowsRequestHandler(IShowInfoRepository showInfoRepository)
        {
            _showInfoRepository = showInfoRepository;
        }
        
        public async Task<IReadOnlyCollection<ApiShow>> Handle(int page, CancellationToken cancellationToken)
        {
            var shows = await _showInfoRepository.ListShows(page, cancellationToken);
            return shows.Select(MapShow).ToList();
        }

        private ApiShow MapShow(Show show)
        {
            return new ApiShow()
            {
                Id = show.ShowId,
                Name = show.Name,
                Cast = show.Cast.OrderByDescending(person => person.Birthday).Select(MapPerson).ToList()
            };
        }

        private ApiPerson MapPerson(Person person)
        {
            return new ApiPerson()
            {
                Id = person.PersonId,
                Name = person.Name,
                Birthday = person.Birthday
            };
        }
    }
}