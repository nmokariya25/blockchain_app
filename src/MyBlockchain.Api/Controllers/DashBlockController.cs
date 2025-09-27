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


        // Common Methods if required 
        // CRUD operations with GetAll 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dashBlocks = await _dashBlockService.GetAllAsync();
            return Ok(dashBlocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dashBlock = await _dashBlockService.GetByIdAsync(id);
            if (dashBlock == null) return NotFound();
            return Ok(dashBlock);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DashBlock dashBlock)
        {
            var created = await _dashBlockService.AddAsync(dashBlock);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DashBlock dashBlock)
        {
            if (id != dashBlock.Id) return BadRequest();

            try
            {
                await _dashBlockService.UpdateAsync(dashBlock);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dashBlockService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }


        // CRUD Operations completed
    }
}
