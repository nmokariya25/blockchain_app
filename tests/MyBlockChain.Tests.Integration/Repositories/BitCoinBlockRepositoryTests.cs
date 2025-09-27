using Microsoft.EntityFrameworkCore;
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
    public class BitCoinBlockRepositoryTests
    {
        private readonly BlockCypherDbContext _context;
        private readonly GenericRepository<BitCoinBlock> _repo;

        public BitCoinBlockRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseSqlite("Filename=BitCoinBlockCypherDb.db")
                .Options;

            _context = new BlockCypherDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repo = new GenericRepository<BitCoinBlock>(_context);
        }

        [Fact]
        public async Task BitCoinBlock_CRUD_Test()
        {
            var bitCoinBlock = new BitCoinBlock
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
            await _repo.AddAsync(bitCoinBlock);
            await _context.SaveChangesAsync();

            var fetched = await _repo.GetByIdAsync(bitCoinBlock.Id);
            Assert.NotNull(fetched);
            Assert.Equal("BTC.test3", fetched.Name);

            fetched.Name = "Updated";
            _repo.Update(fetched);
            await _context.SaveChangesAsync();

            var updated = await _repo.GetByIdAsync(bitCoinBlock.Id);
            Assert.Equal("Updated", updated.Name);

            _repo.Remove(updated);
            await _context.SaveChangesAsync();

            var deleted = await _repo.GetByIdAsync(bitCoinBlock.Id);
            Assert.Null(deleted);
        }
    }
}

