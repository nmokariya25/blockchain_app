using Microsoft.Extensions.Options;
using Moq;
using MyBlockchain.Application.Models;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Unit.Services
{
    public class BitCoinBlockServiceTests
    {
        private readonly BitCoinBlockService _bitCoinBlockService;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<BitCoinBlock>> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<BlockCypherEndPoints>> _mockOptions;

        public BitCoinBlockServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<BitCoinBlock>>();
            _mockUow.Setup(u => u.BitCoinBlocks).Returns(_mockRepo.Object);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<BlockCypherEndPoints>>();
            _mockOptions.Setup(o => o.Value).Returns(new BlockCypherEndPoints());

            _bitCoinBlockService = new BitCoinBlockService(
                _mockHttpClientFactory.Object,
                _mockOptions.Object,
                _mockUow.Object
            );
        }

        [Fact]
        public async Task AddBitCoinBlock_ReturnsBitCoinBlock()
        {
            var BitCoinBlock = new BitCoinBlock
            {
                Id = 1,
                Name = "BTC.test3",
                Height = 23455011,
                Hash = "0xabc123",
                Time = DateTime.UtcNow,
                LatestUrl = "https://api.blockcypher.com/v1/eth/main/blocks/23455011",
                PreviousHash = "0xdef456",
                PreviousUrl = "https://api.blockcypher.com/v1/eth/main/blocks/23455010",
                PeerCount = 50,
                UnconfirmedCount = 1200,
                LastForkHeight = 23400000,
                LastForkHash = "0x789abc",
                CreatedAt = DateTime.UtcNow
            };
            _mockRepo.Setup(r => r.AddAsync(BitCoinBlock)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _bitCoinBlockService.AddAsync(BitCoinBlock);

            Assert.Equal(BitCoinBlock, result);
            _mockRepo.Verify(r => r.AddAsync(BitCoinBlock), Times.Once);
        }
    }
}


