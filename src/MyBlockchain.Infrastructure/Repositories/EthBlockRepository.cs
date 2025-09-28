using Microsoft.EntityFrameworkCore;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;
namespace MyBlockchain.Infrastructure.Repositories
{
    public class EthBlockRepository : GenericRepository<Entity.EthBlock>, IEthBlockRepository
    {
        public EthBlockRepository(BlockCypherDbContext context) : base(context) { }

        public async Task<IEnumerable<Entity.EthBlock>> FetchAllLatestAsync(int count = 0)
        {
            if (count > 0)
            {
                return await _context.EthBlocks
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(count)
                    .ToListAsync();
            }

            return await _context.EthBlocks
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();
        }

    }
}
