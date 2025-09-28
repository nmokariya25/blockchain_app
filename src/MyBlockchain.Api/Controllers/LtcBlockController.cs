using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LtcBlockController : ControllerBase
    {
        private readonly ILtcBlockService _ltcBlockService;
        private readonly ILogger<LtcBlockController> _logger;

        public LtcBlockController(
            ILtcBlockService ltcBlockService,
            ILogger<LtcBlockController> logger)
        {
            _ltcBlockService = ltcBlockService;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var entity = await _ltcBlockService.FetchAndSaveAsync();
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _ltcBlockService.FetchAllLatestAsync(count);

            if (!latestBlock.Any())
            {
                _logger.LogInformation("No block data found in the database.");
                return NoContent();
            }
            return Ok(latestBlock);
        }
    }
}
