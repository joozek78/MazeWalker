using System.Collections.Generic;

namespace MazeWalker.Core.Domain
{
    public class ShowCast
    {
        public ShowCast(IReadOnlyCollection<Person> cast)
        {
            Cast = cast;
        }

        public IReadOnlyCollection<Person> Cast { get; }
    }
}