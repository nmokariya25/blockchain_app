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
        private readonly ILogger<DashBlockController> _logger;
        private readonly IDashBlockService _dashBlockService;

        public DashBlockController(
            IDashBlockService dashBlockService,
            ILogger<DashBlockController> logger)
        {
            this._dashBlockService = dashBlockService;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var entity = await _dashBlockService.FetchAndSaveAsync();
            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _dashBlockService.FetchAllLatestAsync(count);

            if (!latestBlock.Any())
            {
                _logger.LogInformation("No block data found in the database.");
                return NoContent();
            }
            return Ok(latestBlock);
        }


        // Common Methods if required 
        // CRUD operations with GetAll 
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetAll()
        {
            var dashBlocks = await _dashBlockService.GetAllAsync();
            return Ok(dashBlocks);
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetById(int id)
        {
            var dashBlock = await _dashBlockService.GetByIdAsync(id);
            if (dashBlock == null) return NotFound();
            return Ok(dashBlock);
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Create(DashBlock dashBlock)
        {
            var created = await _dashBlockService.AddAsync(dashBlock);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
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
        [ApiExplorerSettings(IgnoreApi = true)]
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
    }
}
