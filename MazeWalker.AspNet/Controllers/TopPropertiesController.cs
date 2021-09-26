using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MazeWalker.Contract;
using MazeWalker.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MazeWalker.AspNet.Controllers
{
    [ApiController]
    [Route("topProperties")]
    public class TopPropertiesController : ControllerBase
    {
        private readonly ILogger<TopPropertiesController> _logger;
        private readonly TopPropertiesHandler _handler;

        public TopPropertiesController(ILogger<TopPropertiesController> logger, TopPropertiesHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpGet]
        [ResponseCache(Duration = 60*5)]
        public async Task<ApiTopPropertiesResponse> Get([FromQuery] string searchTerm,
            [FromQuery] int limit = 10,
            CancellationToken cancellationToken = default)
        {
            return await _handler.Handle(searchTerm, limit, cancellationToken);
        }
    }
}