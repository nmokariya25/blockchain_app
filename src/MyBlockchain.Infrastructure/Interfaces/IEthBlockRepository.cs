using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Interfaces
{
    public interface IEthBlockRepository
    {
        Task<IEnumerable<Entity.EthBlock>> FetchAllLatestAsync(int count = 0);
    }
}
