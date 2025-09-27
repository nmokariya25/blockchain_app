using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<EthBlock> EthBlocks { get; }
        IGenericRepository<DashBlock> DashBlocks { get; }
        IGenericRepository<BtcBlock> BtcBlocks { get; }
        IGenericRepository<LtcBlock> LtcBlocks { get; }

        Task<int> CompleteAsync();
    }
}
