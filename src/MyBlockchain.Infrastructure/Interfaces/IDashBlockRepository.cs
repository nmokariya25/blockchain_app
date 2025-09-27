using MyBlockchain.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Interfaces
{
    public interface IDashBlockRepository : IGenericRepository<Entity.DashBlock>
    {
        Task<IEnumerable<Entity.DashBlock>> FetchAllLatestAsync(int count = 0);
    }
}
