using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Services
{
    public class LtcBlockService : ILtcBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;

        public LtcBlockService(IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LtcBlock>> GetAllAsync()
        {
            return await _unitOfWork.LtcBlocks.GetAllAsync();
        }

        public async Task<LtcBlock?> GetByIdAsync(int id)
        {
            var block = await _unitOfWork.LtcBlocks.GetByIdAsync(id);
            if (block == null)
                throw new KeyNotFoundException("Block not found");
            return block;
        }

        public async Task<LtcBlock> AddAsync(LtcBlock LtcBlock)
        {
            await _unitOfWork.LtcBlocks.AddAsync(LtcBlock);
            await _unitOfWork.CompleteAsync();
            return LtcBlock;
        }

        public async Task UpdateAsync(LtcBlock LtcBlock)
        {
            _unitOfWork.LtcBlocks.Update(LtcBlock);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var LtcBlock = await _unitOfWork.LtcBlocks.GetByIdAsync(id);
            if (LtcBlock == null) throw new KeyNotFoundException("LtcBlock not found");
            _unitOfWork.LtcBlocks.Remove(LtcBlock);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<LtcBlockDto> GetLatestBlockAsync()
        {
            var LtcBlockClient = _httpClientFactory.CreateClient("LtcBlockClient");
            var url = _blockCypherEndPoints.LtcBlock;
            var response = await LtcBlockClient.GetFromJsonAsync<LtcBlockDto>(url);
            if (response is null)
                throw new InvalidOperationException("Failed to retrieve the latest Ethereum block.");
            return response;
        }
    }
}
