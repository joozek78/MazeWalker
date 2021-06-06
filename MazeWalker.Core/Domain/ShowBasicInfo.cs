using System;

namespace MazeWalker.Core.Domain
{
    public class ShowBasicInfo
    {
        public ShowBasicInfo(int showId, string name)
        {
            ShowId = showId;
            Name = name;
        }
        public int ShowId { get; }
        public string Name { get; }

        protected bool Equals(ShowBasicInfo other)
        {
            return ShowId == other.ShowId && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShowBasicInfo) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShowId, Name);
        }
    }
}