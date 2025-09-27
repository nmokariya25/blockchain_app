using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Services
{
    public class DashBlockService : IDashBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashBlockRepository _dashBlockRepository;

        public DashBlockService(
            IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            IDashBlockRepository dashBlockRepository)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _dashBlockRepository = dashBlockRepository;
        }

        public async Task<IEnumerable<DashBlock>> GetAllAsync()
        {
            return await _dashBlockRepository.GetAllAsync();
        }

        public async Task<DashBlock?> GetByIdAsync(int id)
        {
            var block = await _unitOfWork.DashBlocks.GetByIdAsync(id);
            if (block == null)
                throw new KeyNotFoundException("Block not found");
            return block;
        }

        public async Task<DashBlock> AddAsync(DashBlock DashBlock)
        {
            await _unitOfWork.DashBlocks.AddAsync(DashBlock);
            await _unitOfWork.CompleteAsync();
            return DashBlock;
        }

        public async Task UpdateAsync(DashBlock DashBlock)
        {
            _unitOfWork.DashBlocks.Update(DashBlock);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var DashBlock = await _unitOfWork.DashBlocks.GetByIdAsync(id);
            if (DashBlock == null) throw new KeyNotFoundException("DashBlock not found");
            _unitOfWork.DashBlocks.Remove(DashBlock);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<DashBlockDto> GetLatestBlockAsync()
        {
            var DashBlockClient = _httpClientFactory.CreateClient("DashBlockClient");
            var url = _blockCypherEndPoints.DashBlock;
            var response = await DashBlockClient.GetFromJsonAsync<DashBlockDto>(url);
            if (response is null)
                throw new InvalidOperationException("Failed to retrieve the latest Ethereum block.");
            return response;
        }

        public async Task<IEnumerable<DashBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.DashBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }
}
