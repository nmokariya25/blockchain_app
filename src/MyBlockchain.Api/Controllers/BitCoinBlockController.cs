using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitCoinBlockController : ControllerBase
    {
        private readonly IBitCoinBlockService _bitCoinBlockService;
        private readonly ILogger<BitCoinBlockController> _logger;

        public BitCoinBlockController(
            IBitCoinBlockService BitCoinBlockService,
            ILogger<BitCoinBlockController> logger)
        {
            _bitCoinBlockService = BitCoinBlockService;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var entity = await _bitCoinBlockService.FetchAndSaveAsync();
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _bitCoinBlockService.FetchAllLatestAsync(count);

            if (!latestBlock.Any())
            {
                _logger.LogInformation("No block data found in the database.");
                return NoContent();
            }
            return Ok(latestBlock);
        }
    }
}

