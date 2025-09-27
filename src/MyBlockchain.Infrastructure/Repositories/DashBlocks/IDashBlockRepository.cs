using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Repositories.DashBlocks
{
    public interface IDashBlockRepository : IGenericRepository<Entity.DashBlock>
    {
        Task<IEnumerable<Entity.DashBlock>> FetchAllLatestAsync(int count = 0);
    }
}
