using AutoMapper;
using Microsoft.Extensions.Configuration;
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
        private readonly ILogger<EthBlockService> _logger;
        private readonly IMapper _mapper;

        public EthBlockService(
            IHttpClientFactory httpClientFactory,
            IOptions<BlockCypherEndPoints> blockCypherEndPoints,
            IUnitOfWork unitOfWork,
            ILogger<EthBlockService> logger,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<EthBlock> FetchAndSaveAsync()
        {
            try
            {
                using (var ethBlockClient = _httpClientFactory.CreateClient("EthBlockClient"))
                {
                    var url = _blockCypherEndPoints.EthBlock;
                    var response = await ethBlockClient.GetFromJsonAsync<EthBlockDto>(url);
                    if (response == null)
                        throw new HttpRequestException();

                    var objDashBlock = _mapper.Map<EthBlock>(response);
                    return await AddAsync(objDashBlock);
                }
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


        public async Task<IEnumerable<EthBlock>> FetchAllLatestAsync(int count = 0)
        {
            var latestBlocks = await _unitOfWork.EthBlockRepository.FetchAllLatestAsync(count);
            return latestBlocks;
        }
    }

}
