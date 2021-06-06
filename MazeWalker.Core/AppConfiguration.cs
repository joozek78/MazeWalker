namespace MazeWalker.Core
{
    public class AppConfiguration
    {
        public int MaxPagesPerInvocation { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string CosmosAccountName { get; set; }
        public string CosmosDatabaseName { get; set; }
    }
}