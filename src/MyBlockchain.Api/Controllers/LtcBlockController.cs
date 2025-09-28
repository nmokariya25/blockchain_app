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
        private readonly IMapper _mapper;

        public LtcBlockController(
            ILtcBlockService ltcBlockService,
            IMapper mapper)
        {
            _ltcBlockService = ltcBlockService;
            _mapper = mapper;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var dto = await _ltcBlockService.GetLatestBlockAsync();
            if (dto == null) return NotFound("No data received from API.");
            var entity = _mapper.Map<LtcBlock>(dto);
            await _ltcBlockService.AddAsync(entity);
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _ltcBlockService.GetAllAsync();
            var blockHistory = (count > 0)
                ? latestBlock.OrderByDescending(b => b.CreatedAt).Take(count)
                : latestBlock.OrderByDescending(b => b.CreatedAt);
            if (!blockHistory.Any())
                return NotFound("No block data found.");
            return Ok(blockHistory);
        }
    }
}
