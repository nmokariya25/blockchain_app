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
    public class DashBlockRepositoryTests
    {
        private readonly BlockCypherDbContext _context;
        private readonly GenericRepository<DashBlock> _repo;

        public DashBlockRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseSqlite("Filename=DashBlockCypherDb.db")
                .Options;

            _context = new BlockCypherDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repo = new GenericRepository<DashBlock>(_context);
        }

        [Fact]
        public async Task DashBlock_CRUD_Test()
        {
            var DashBlock = new DashBlock
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
            await _repo.AddAsync(DashBlock);
            await _context.SaveChangesAsync();

            var fetched = await _repo.GetByIdAsync(DashBlock.Id);
            Assert.NotNull(fetched);
            Assert.Equal("DASH.main", fetched.Name);

            fetched.Name = "Updated";
            _repo.Update(fetched);
            await _context.SaveChangesAsync();

            var updated = await _repo.GetByIdAsync(DashBlock.Id);
            Assert.Equal("Updated", updated.Name);

            _repo.Remove(updated);
            await _context.SaveChangesAsync();

            var deleted = await _repo.GetByIdAsync(DashBlock.Id);
            Assert.Null(deleted);
        }
    }
}
