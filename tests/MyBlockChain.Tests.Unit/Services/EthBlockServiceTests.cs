using Microsoft.Extensions.Options;
using Moq;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Unit.Services
{
    public class EthBlockServiceTests
    {
        private readonly EthBlockService _ethBlockService;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<EthBlock>> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<BlockCypherEndPoints>> _mockOptions;

        public EthBlockServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<EthBlock>>();
            _mockUow.Setup(u => u.EthBlocks).Returns(_mockRepo.Object);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<BlockCypherEndPoints>>();
            _mockOptions.Setup(o => o.Value).Returns(new BlockCypherEndPoints());

            _ethBlockService = new EthBlockService(
                _mockHttpClientFactory.Object,
                _mockOptions.Object,
                _mockUow.Object
            );
        }

        [Fact]
        public async Task AddEthBlock_ReturnsEthBlock()
        {
            var ethBlock = new EthBlock
            {
                Id = 1,
                Name = "ETH.main",
                Height = 23455011,
                Hash = "0xabc123",
                Time = DateTime.UtcNow,
                LatestUrl = "https://api.blockcypher.com/v1/eth/main/blocks/23455011",  
                PreviousHash = "0xdef456",
                PreviousUrl = "https://api.blockcypher.com/v1/eth/main/blocks/23455010",    
                PeerCount = 50,
                UnconfirmedCount = 1200,
                HighGasPrice = 200,
                MediumGasPrice = 100,
                LowGasPrice = 50,
                HighPriorityFee = 150,
                MediumPriorityFee = 75,
                LowPriorityFee = 30,
                BaseFee = 90,
                LastForkHeight = 23400000,
                LastForkHash = "0x789abc",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.AddAsync(ethBlock)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _ethBlockService.AddAsync(ethBlock);

            Assert.Equal(ethBlock, result);
            _mockRepo.Verify(r => r.AddAsync(ethBlock), Times.Once);
        }
    }
}
