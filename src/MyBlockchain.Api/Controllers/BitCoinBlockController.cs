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
        private readonly IMapper _mapper;

        public BitCoinBlockController(
            IBitCoinBlockService BitCoinBlockService,
            IMapper mapper)
        {
            this._bitCoinBlockService = BitCoinBlockService;
            this._mapper = mapper;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var dto = await _bitCoinBlockService.GetLatestBlockAsync();
            if (dto == null) return NotFound("No data received from API.");
            var entity = _mapper.Map<BitCoinBlock>(dto);
            await _bitCoinBlockService.AddAsync(entity);
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _bitCoinBlockService.GetAllAsync();
            var blockHistory = (count > 0)
                ? latestBlock.OrderByDescending(b => b.CreatedAt).Take(count)
                : latestBlock.OrderByDescending(b => b.CreatedAt);
            if (!blockHistory.Any())
                return NotFound("No block data found.");
            return Ok(blockHistory);
        }
    }
}

