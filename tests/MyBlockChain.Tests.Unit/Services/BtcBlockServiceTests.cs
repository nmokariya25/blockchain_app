using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using MyBlockchain.Application.AutoMappers;
using MyBlockchain.Application.Models;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Unit.Services
{
    public class BtcBlockServiceTests
    {
        private readonly BtcBlockService _btcBlockService;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<BtcBlock>> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<BlockCypherEndPoints>> _mockOptions;
        private readonly IMapper _mapper;

        public BtcBlockServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<BtcBlock>>();
            _mockUow.Setup(u => u.BtcBlocks).Returns(_mockRepo.Object);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<BlockCypherEndPoints>>();
            _mockOptions.Setup(o => o.Value).Returns(new BlockCypherEndPoints());

            _btcBlockService = new BtcBlockService(
                _mockHttpClientFactory.Object,
                _mockOptions.Object,
                _mockUow.Object,
                new FakeLogger<BtcBlockService>(),
                _mapper = config.CreateMapper()
            );
        }

        [Fact]
        public async Task AddBtcBlock_ReturnsBtcBlock()
        {
            var btcBlock = new BtcBlock
            {
                Id = 1,
                Name = "BTC.main",
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
            _mockRepo.Setup(r => r.AddAsync(btcBlock)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _btcBlockService.AddAsync(btcBlock);

            Assert.Equal(btcBlock, result);
            _mockRepo.Verify(r => r.AddAsync(btcBlock), Times.Once);
        }

        [Fact]
        public async Task BtcBlockApi_ShouldReturnStatus200()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            using (var btcBlockClient = httpClientFactory.CreateClient("BtcBlockClientTest"))
            {
                var btcBlockApiUrl = "https://api.blockcypher.com/v1/btc/main";
                var response = await btcBlockClient.GetAsync(btcBlockApiUrl);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }   
        }
    }
}

