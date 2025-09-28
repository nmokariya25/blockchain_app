using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthBlockController : ControllerBase
    {
        private readonly IEthBlockService _ethBlockService;
        private readonly ILogger<EthBlockController> _logger;

        public EthBlockController(
            IEthBlockService ethBlockService, 
            ILogger<EthBlockController> logger)
        {
            _ethBlockService = ethBlockService;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var entity = await _ethBlockService.FetchAndSaveAsync();
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _ethBlockService.FetchAllLatestAsync(count);

            if (!latestBlock.Any())
            {
                _logger.LogInformation("No block data found in the database.");
                return NoContent();
            }
            return Ok(latestBlock);
        }
    }

}
