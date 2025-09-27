using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthBlockController : ControllerBase
    {
        private readonly IEthBlockService _ethService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EthBlockController(IEthBlockService ethService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ethService = ethService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("save-latest-block")]
        public async Task<IActionResult> SaveLatestBlock()
        {
            var dto = await _ethService.GetLatestBlockAsync();
            if (dto == null) return NotFound("No data received from API.");

            var entity = _mapper.Map<EthBlock>(dto);
            await _unitOfWork.EthBlocks.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Block data saved successfully", blockHeight = entity.Height });
        }

        [HttpGet("latest-block/{count?}")]
        public async Task<IActionResult> GetLatestBlock(int count = 0)
        {
            var latestBlock = await _unitOfWork.EthBlocks.GetAllAsync();

            var blockHistory = (count > 0)
                ? latestBlock.OrderByDescending(b => b.CreatedAt).Take(count)
                : latestBlock.OrderByDescending(b => b.CreatedAt);

            if (!blockHistory.Any())
                return NotFound("No block data found.");

            return Ok(blockHistory);
        }
    }

}
