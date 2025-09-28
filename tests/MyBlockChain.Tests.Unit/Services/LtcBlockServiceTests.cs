using AutoMapper;
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
    public class LtcBlockServiceTests
    {
        private readonly LtcBlockService _ltcBlockService;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<LtcBlock>> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<BlockCypherEndPoints>> _mockOptions;
        private readonly ILtcBlockRepository _mockLtcBlockRepo;
        private readonly IMapper _mapper;

        public LtcBlockServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<LtcBlock>>();
            _mockUow.Setup(u => u.LtcBlocks).Returns(_mockRepo.Object);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<BlockCypherEndPoints>>();
            _mockOptions.Setup(o => o.Value).Returns(new BlockCypherEndPoints());

            _ltcBlockService = new LtcBlockService(
                _mockHttpClientFactory.Object,
                _mockOptions.Object,
                _mockUow.Object,
                new FakeLogger<LtcBlockService>(),
                _mapper = config.CreateMapper()
            );
        }

        [Fact]
        public async Task AddLtcBlock_ReturnsLtcBlock()
        {
            var ltcBlock = new LtcBlock
            {
                Id = 1,
                Name = "LTC.main",
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
            _mockRepo.Setup(r => r.AddAsync(ltcBlock)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _ltcBlockService.AddAsync(ltcBlock);

            Assert.Equal(ltcBlock, result);
            _mockRepo.Verify(r => r.AddAsync(ltcBlock), Times.Once);
        }
    }
}


