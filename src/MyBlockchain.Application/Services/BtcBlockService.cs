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
    public class BtcBlockService : IBtcBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;

        public BtcBlockService(IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BtcBlock>> GetAllAsync()
        {
            return await _unitOfWork.BtcBlocks.GetAllAsync();
        }

        public async Task<BtcBlock?> GetByIdAsync(int id)
        {
            var block = await _unitOfWork.BtcBlocks.GetByIdAsync(id);
            if (block == null)
                throw new KeyNotFoundException("Block not found");
            return block;
        }

        public async Task<BtcBlock> AddAsync(BtcBlock BtcBlock)
        {
            await _unitOfWork.BtcBlocks.AddAsync(BtcBlock);
            await _unitOfWork.CompleteAsync();
            return BtcBlock;
        }

        public async Task UpdateAsync(BtcBlock BtcBlock)
        {
            _unitOfWork.BtcBlocks.Update(BtcBlock);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var BtcBlock = await _unitOfWork.BtcBlocks.GetByIdAsync(id);
            if (BtcBlock == null) throw new KeyNotFoundException("BtcBlock not found");
            _unitOfWork.BtcBlocks.Remove(BtcBlock);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<BtcBlockDto> GetLatestBlockAsync()
        {
            var BtcBlockClient = _httpClientFactory.CreateClient("BtcBlockClient");
            var url = _blockCypherEndPoints.BtcBlock;
            var response = await BtcBlockClient.GetFromJsonAsync<BtcBlockDto>(url);
            if (response is null)
                throw new InvalidOperationException("Failed to retrieve the latest Ethereum block.");
            return response;
        }
    }
}
