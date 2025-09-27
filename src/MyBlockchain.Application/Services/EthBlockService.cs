using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Services
{
    public class EthBlockService : IEthBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;

        public EthBlockService(
            IHttpClientFactory httpClientFactory, 
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EthBlock>> GetAllAsync()
        {
            return await _unitOfWork.EthBlocks.GetAllAsync();
        }

        public async Task<EthBlock?> GetByIdAsync(int id)
        {
            var block = await _unitOfWork.EthBlocks.GetByIdAsync(id);
            if (block == null)
                throw new KeyNotFoundException("Block not found");
            return block;
        }

        public async Task<EthBlock> AddAsync(EthBlock ethBlock)
        {
            await _unitOfWork.EthBlocks.AddAsync(ethBlock);
            await _unitOfWork.CompleteAsync();
            return ethBlock;
        }

        public async Task UpdateAsync(EthBlock ethBlock)
        {
            _unitOfWork.EthBlocks.Update(ethBlock);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ethBlock = await _unitOfWork.EthBlocks.GetByIdAsync(id);
            if (ethBlock == null) throw new KeyNotFoundException("EthBlock not found");
            _unitOfWork.EthBlocks.Remove(ethBlock);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<EthBlockDto> GetLatestBlockAsync()
        {
            var ethBlockClient = _httpClientFactory.CreateClient("EthBlockClient");
            var url = _blockCypherEndPoints.EthBlock;
            var response = await ethBlockClient.GetFromJsonAsync<EthBlockDto>(url);
            if (response is null)
                throw new InvalidOperationException("Failed to retrieve the latest Ethereum block.");
            return response;
        }
    }

}
