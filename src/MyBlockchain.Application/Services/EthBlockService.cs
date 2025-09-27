using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyBlockchain.Application.DTOs;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
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

        public EthBlockService(IHttpClientFactory httpClientFactory, IOptions<BlockCypherEndPoints> blockCypherEndPoints)
        {
            _httpClientFactory = httpClientFactory;
            _blockCypherEndPoints = blockCypherEndPoints.Value;
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
