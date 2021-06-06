using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeWalker.Core.Domain
{
    public class Show
    {
        public Show(int showId, string name, IReadOnlyCollection<Person> cast)
        {
            ShowId = showId;
            Name = name;
            Cast = cast;
        }

        public Show(ShowBasicInfo showBasicInfo, IReadOnlyCollection<Person> cast)
        {
            ShowId = showBasicInfo.ShowId;
            Name = showBasicInfo.Name;
            Cast = cast;
        }

        public int ShowId { get; }
        public string Name { get; }
        public IReadOnlyCollection<Person> Cast { get; }

        protected bool Equals(Show other)
        {
            return ShowId == other.ShowId && Name == other.Name && Cast.SequenceEqual(other.Cast);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Show) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShowId, Name, Cast);
        }
    }
}