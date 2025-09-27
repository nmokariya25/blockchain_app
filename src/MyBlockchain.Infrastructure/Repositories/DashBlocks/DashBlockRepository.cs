using Microsoft.EntityFrameworkCore;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Repositories.DashBlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Repositories.DashBlock
{
    public class DashBlockRepository : GenericRepository<Entity.DashBlock>, IDashBlockRepository
    {
        public DashBlockRepository(BlockCypherDbContext context) : base(context) { }

        public async Task<IEnumerable<Entity.DashBlock>> FetchAllLatestAsync(int count = 0)
        {
            if (count > 0)
            {
                return await _context.DashBlocks
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(count)
                    .ToListAsync();
            }

            return await _context.DashBlocks
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();
        }

    }
}
