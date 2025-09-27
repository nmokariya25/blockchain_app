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
    public class BitCoinBlockService : IBitCoinBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;

        public BitCoinBlockService(IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BitCoinBlock>> GetAllAsync()
        {
            return await _unitOfWork.BitCoinBlocks.GetAllAsync();
        }

        public async Task<BitCoinBlock?> GetByIdAsync(int id)
        {
            var block = await _unitOfWork.BitCoinBlocks.GetByIdAsync(id);
            if (block == null)
                throw new KeyNotFoundException("Block not found");
            return block;
        }

        public async Task<BitCoinBlock> AddAsync(BitCoinBlock BitCoinBlock)
        {
            await _unitOfWork.BitCoinBlocks.AddAsync(BitCoinBlock);
            await _unitOfWork.CompleteAsync();
            return BitCoinBlock;
        }

        public async Task UpdateAsync(BitCoinBlock BitCoinBlock)
        {
            _unitOfWork.BitCoinBlocks.Update(BitCoinBlock);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var BitCoinBlock = await _unitOfWork.BitCoinBlocks.GetByIdAsync(id);
            if (BitCoinBlock == null) throw new KeyNotFoundException("BitCoinBlock not found");
            _unitOfWork.BitCoinBlocks.Remove(BitCoinBlock);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<BitCoinBlockDto> GetLatestBlockAsync()
        {
            var bitCoinBlockClient = _httpClientFactory.CreateClient("BitCoinBlockClient");
            var url = _blockCypherEndPoints.BitCoinBlock;
            var response = await bitCoinBlockClient.GetFromJsonAsync<BitCoinBlockDto>(url);
            if (response is null)
                throw new InvalidOperationException("Failed to retrieve the latest Ethereum block.");
            return response;
        }
    }
}
