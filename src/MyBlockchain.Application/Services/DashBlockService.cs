using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Extensions;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Services
{
    public class DashBlockService : IDashBlockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BlockCypherEndPoints _blockCypherEndPoints;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDashBlockRepository _dashBlockRepository;
        private readonly ILogger<DashBlockService> _logger;
        private readonly IMapper _mapper;

        public DashBlockService(
            IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            IDashBlockRepository dashBlockRepository,
            ILogger<DashBlockService> logger,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _dashBlockRepository = dashBlockRepository;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<DashBlockDto> GetDashBlockDataAsync()
        {
            using (var dashBlockClient = _httpClientFactory.CreateClient("DashBlockClient"))
            {
                var url = _blockCypherEndPoints.DashBlock;
                var dashBlock = await dashBlockClient.GetFromJsonAsync<DashBlockDto>(url);
                if (dashBlock == null)
                    throw new HttpRequestException("Failed to retrieve DashBlockDto from API.");
                return dashBlock;
            }
        }

        public async Task<DashBlock> FetchAndSaveAsync()
        {
            try
            {
                var response = await GetDashBlockDataAsync();
                var objDashBlock = _mapper.Map<DashBlock>(response);
                return await AddAsync(objDashBlock);
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

        public async Task<IEnumerable<DashBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.DashBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }
}
