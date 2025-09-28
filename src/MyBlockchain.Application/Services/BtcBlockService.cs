using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
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
    public class BtcBlockService : IBtcBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BtcBlockService> _logger;
        private readonly IMapper _mapper;

        public BtcBlockService(
            IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            ILogger<BtcBlockService> logger,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<BtcBlock> FetchAndSaveAsync()
        {
            try
            {
                var btcBlockClient = _httpClientFactory.CreateClient("BtcBlockClient");
                var url = _blockCypherEndPoints.BtcBlock;
                var response = await btcBlockClient.GetFromJsonAsync<BtcBlockDto>(url);
                if (response == null)
                    throw new HttpRequestException();

                var objBtcBlock = _mapper.Map<BtcBlock>(response);
                return await AddAsync(objBtcBlock);
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

        public async Task<IEnumerable<BtcBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.BtcBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }
}
