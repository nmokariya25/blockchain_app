using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
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
    public class DashBlockServiceTests
    {
        private readonly DashBlockService _dashBlockService;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<DashBlock>> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<BlockCypherEndPoints>> _mockOptions;
        private readonly IDashBlockRepository _mockDashRepo;
        private readonly IMapper _mapper;

        public DashBlockServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<DashBlock>>();
            _mockDashRepo = new Mock<IDashBlockRepository>().Object;
            _mockUow.Setup(u => u.DashBlocks).Returns(_mockRepo.Object);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<BlockCypherEndPoints>>();
            _mockOptions.Setup(o => o.Value).Returns(new BlockCypherEndPoints());

            _dashBlockService = new DashBlockService(
                _mockHttpClientFactory.Object,
                _mockOptions.Object,
                _mockUow.Object,
                _mockDashRepo,
                new FakeLogger<DashBlockService>(),
                _mapper = config.CreateMapper()
            );
        }

        [Fact]
        public async Task AddDashBlock_ReturnsDashBlock()
        {   
            var dashBlock = new DashBlock
            {
                Id = 1,
                Name = "DASH.main",
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
            _mockRepo.Setup(r => r.AddAsync(dashBlock)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _dashBlockService.AddAsync(dashBlock);

            Assert.Equal(dashBlock, result);
            _mockRepo.Verify(r => r.AddAsync(dashBlock), Times.Once);
        }

        [Fact]
        public async Task DashBlockApi_ShouldReturnStatus200()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            using (var dashBlockClient = httpClientFactory.CreateClient("DashBlockClientTest"))
            {
                var dashBlockApiUrl = "https://api.blockcypher.com/v1/dash/main";
                var response = await new HttpClient().GetAsync(dashBlockApiUrl);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
