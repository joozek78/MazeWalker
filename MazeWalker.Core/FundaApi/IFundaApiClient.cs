using System.Threading.Tasks;

namespace MazeWalker.Core.FundaApi
{
    public interface IFundaApiClient
    {
        Task<PropertiesPage> SearchProperties(string searchTerm, int pageBase1);
    }
}