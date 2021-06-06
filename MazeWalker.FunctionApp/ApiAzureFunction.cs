using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Core.MazeWalkerApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace MazeWalker.FunctionApp
{
    public class ApiAzureFunction
    {
        private readonly GetShowsRequestHandler _handler;

        public ApiAzureFunction(GetShowsRequestHandler handler)
        {
            _handler = handler;
        }
        
        [FunctionName("GetShows")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "shows/{page:int}")] HttpRequest request,
            int page,
            CancellationToken cancellationToken)
        {
            var apiShows = await _handler.Handle(page, cancellationToken);
            return new OkObjectResult(apiShows);
        }
    }
}