using System.Threading;
using System.Threading.Tasks;

namespace MazeWalker.Core.FundaApi
{
    public interface IFundaApiClient
    {
        Task<PropertiesPage> SearchProperties(string searchTerm, int pageBase1, CancellationToken cancellationToken = default);
    }
}