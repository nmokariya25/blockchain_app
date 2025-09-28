using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Extensions;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.UnitOfWork;
using Newtonsoft.Json;
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
        private readonly ILogger<BitCoinBlockService> _logger;
        private readonly IMapper _mapper;

        public BitCoinBlockService(IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            ILogger<BitCoinBlockService> logger,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<BitCoinBlock> FetchAndSaveAsync()
        {
            try
            {
                var bitCoinBlockClient = _httpClientFactory.CreateClient("BitCoinBlockClient");
                var url = _blockCypherEndPoints.BitCoinBlock;
                var response = await bitCoinBlockClient.GetFromJsonAsync<BitCoinBlockDto>(url);
                if (response == null)
                    throw new HttpRequestException();

                var objBitCoinBlock = _mapper.Map<BitCoinBlock>(response);
                return await AddAsync(objBitCoinBlock);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"There is some issue while retriving the data from Api. Detailed Exception is: {JsonConvert.SerializeObject(ex)}");
                throw new HttpRequestException("Failed to retrive the data from Api");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is some issue while saving the records. Exception is: {JsonConvert.SerializeObject(ex)}");
                throw new Exception("Failed to save the latest Dashblock.");
            }
        }

        public async Task<IEnumerable<BitCoinBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.BitCoinBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }
}
