namespace MazeWalker.Adapters.Cosmos
{
    public static class CosmosShowPages
    {
        public const int PageSize = 20;
        public const int FirstPage = 0;
        
        public static (int minExclusive, int maxInclusive) GetIndexBoundsForPage(int page)
        {
            return (PageSize * page, PageSize * (page + 1));
        }
    }
}