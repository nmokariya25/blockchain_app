using Microsoft.EntityFrameworkCore;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Interfaces;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Repositories
{
    public class ApiAuditRepository : GenericRepository<Entity.ApiAuditLog>, IApiAuditRepository
    {
        public ApiAuditRepository(BlockCypherDbContext context) : base(context) { }

        public async Task<IEnumerable<Entity.ApiAuditLog>> FetchAllLatestAsync(int count = 0)
        {
            var query = _context.ApiAudits.OrderByDescending(a => a.RequestDate);

            if (count > 0)
                return await query.Take(count).ToListAsync();

            return await query.ToListAsync();
        }
    }
}
