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
    public class LtcBlockService : ILtcBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LtcBlockService> _logger;
        private readonly IMapper _mapper;

        public LtcBlockService(
            IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            ILogger<LtcBlockService> logger,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<LtcBlock> FetchAndSaveAsync()
        {
            try
            {
                var ltcBlockClient = _httpClientFactory.CreateClient("LtcBlockClient");
                var url = _blockCypherEndPoints.EthBlock;
                var response = await ltcBlockClient.GetFromJsonAsync<LtcBlockDto>(url);
                if (response == null)
                    throw new HttpRequestException();

                var obcLtcBlock = _mapper.Map<LtcBlock>(response);
                return await AddAsync(obcLtcBlock);
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

        public async Task<IEnumerable<LtcBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.LtcBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }

}
