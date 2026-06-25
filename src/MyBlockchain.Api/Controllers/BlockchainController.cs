using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IBlockchainOrchestrationService _orchestrationService;

        public BlockchainController(IBlockchainOrchestrationService orchestrationService)
        {
            _orchestrationService = orchestrationService;
        }

        /// <summary>
        /// Fetches the latest block data from all BlockCypher chains in parallel and stores them in the database.
        /// </summary>
        [HttpPost("fetch-all")]
        public async Task<IActionResult> FetchAllChains()
        {
            var results = await _orchestrationService.FetchAllChainsParallelAsync();
            return Ok(new { message = "All blockchain data fetched and saved successfully", blockHeights = results });
        }
    }
}
