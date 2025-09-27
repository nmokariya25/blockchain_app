using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBlockController : ControllerBase
    {
        private readonly IDashBlockService _dashBlockService;
        private readonly IMapper _mapper;

        public DashBlockController(
            IDashBlockService dashBlockService,
            IMapper mapper)
        {
            this._dashBlockService = dashBlockService;
            this._mapper = mapper;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var dto = await _dashBlockService.GetLatestBlockAsync();
            if (dto == null) return NotFound("No data received from API.");

            var entity = _mapper.Map<DashBlock>(dto);
            await _dashBlockService.AddAsync(entity);

            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _dashBlockService.GetAllAsync();

            var blockHistory = (count > 0)
                ? latestBlock.OrderByDescending(b => b.CreatedAt).Take(count)
                : latestBlock.OrderByDescending(b => b.CreatedAt);

            if (!blockHistory.Any())
                return NotFound("No block data found.");

            return Ok(blockHistory);
        }
    }
}
