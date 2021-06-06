using System.Collections.Generic;

namespace MazeWalker.Core.MazeWalkerApi.Models
{
    public class ApiShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyCollection<ApiPerson> Cast { get; set; }
    }
}