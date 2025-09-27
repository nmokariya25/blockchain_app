using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Integration.Repositories
{
    public class EthBlockRepositoryTests
    {
        private readonly BlockCypherDbContext _context;
        private readonly GenericRepository<EthBlock> _repo;

        public EthBlockRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseSqlite("Filename=TestBlockCypherDb.db")
                .Options;

            _context = new BlockCypherDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repo = new GenericRepository<EthBlock>(_context);
        }

        [Fact]
        public async Task EthBlock_CRUD_Test()
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
            await _repo.AddAsync(ethBlock);
            await _context.SaveChangesAsync();

            var fetched = await _repo.GetByIdAsync(ethBlock.Id);
            Assert.NotNull(fetched);
            Assert.Equal("ETH.main", fetched.Name);

            fetched.Name = "Updated";
            _repo.Update(fetched);
            await _context.SaveChangesAsync();

            var updated = await _repo.GetByIdAsync(ethBlock.Id);
            Assert.Equal("Updated", updated.Name);

            _repo.Remove(updated);
            await _context.SaveChangesAsync();

            var deleted = await _repo.GetByIdAsync(ethBlock.Id);
            Assert.Null(deleted);
        }
    }
}
