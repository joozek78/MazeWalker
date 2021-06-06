namespace MazeWalker.Core.ScraperState
{
    public class ScraperState
    {
        public ScraperState()
        {
            CurrentPageNumber = 0;
        }

        public ScraperState(int currentPageNumber)
        {
            CurrentPageNumber = currentPageNumber;
        }
        
        public int CurrentPageNumber { get; }

        public ScraperState WithNextPage() => new ScraperState(CurrentPageNumber + 1);

        protected bool Equals(ScraperState other)
        {
            return CurrentPageNumber == other.CurrentPageNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ScraperState) obj);
        }

        public override int GetHashCode()
        {
            return CurrentPageNumber;
        }
    }
}