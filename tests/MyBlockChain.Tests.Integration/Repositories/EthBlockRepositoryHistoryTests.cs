using Microsoft.EntityFrameworkCore;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;

namespace MyBlockChain.Tests.Integration.Repositories
{
    public class EthBlockRepositoryHistoryTests
    {
        private readonly BlockCypherDbContext _context;
        private readonly EthBlockRepository _repository;

        public EthBlockRepositoryHistoryTests()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            _context = new BlockCypherDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _repository = new EthBlockRepository(_context);
        }

        [Fact]
        public async Task FetchAllLatestAsync_Returns_Blocks_Ordered_By_CreatedAt_Descending()
        {
            var older = CreateEthBlock(100, DateTime.UtcNow.AddHours(-2));
            var newer = CreateEthBlock(200, DateTime.UtcNow.AddHours(-1));
            var newest = CreateEthBlock(300, DateTime.UtcNow);

            _context.EthBlocks.AddRange(older, newer, newest);
            await _context.SaveChangesAsync();

            var results = (await _repository.FetchAllLatestAsync()).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal(300, results[0].Height);
            Assert.Equal(200, results[1].Height);
            Assert.Equal(100, results[2].Height);
        }

        [Fact]
        public async Task FetchAllLatestAsync_Respects_Count_Parameter()
        {
            for (var i = 1; i <= 5; i++)
            {
                _context.EthBlocks.Add(CreateEthBlock(i, DateTime.UtcNow.AddMinutes(i)));
            }
            await _context.SaveChangesAsync();

            var results = (await _repository.FetchAllLatestAsync(2)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal(5, results[0].Height);
            Assert.Equal(4, results[1].Height);
        }

        private static EthBlock CreateEthBlock(long height, DateTime createdAt) => new()
        {
            Name = "ETH.main",
            Height = height,
            Hash = $"hash-{height}",
            Time = createdAt,
            PeerCount = 1,
            UnconfirmedCount = 1,
            HighGasPrice = 1,
            MediumGasPrice = 1,
            LowGasPrice = 1,
            HighPriorityFee = 1,
            MediumPriorityFee = 1,
            LowPriorityFee = 1,
            BaseFee = 1,
            LastForkHeight = 1,
            CreatedAt = createdAt
        };
    }
}
