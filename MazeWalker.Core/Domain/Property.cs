using System;

namespace MazeWalker.Core.Domain
{
    public class Property
    {
        public Property(string url, string address, string globalId, Agent agent)
        {
            URL = url ?? throw new ArgumentNullException(nameof(url));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            GlobalId = globalId ?? throw new ArgumentNullException(nameof(globalId));
            Agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public string URL { get; }
        
        public string Address { get; }
        
        public string GlobalId { get; }

        public Agent Agent { get; }
    }
}