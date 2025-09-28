using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Interfaces
{
    public interface IBtcBlockRepository
    {
        Task<IEnumerable<Entity.BtcBlock>> FetchAllLatestAsync(int count = 0);
    }
}
