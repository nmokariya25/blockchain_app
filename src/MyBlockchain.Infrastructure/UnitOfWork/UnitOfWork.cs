using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Interfaces;
using MyBlockchain.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlockCypherDbContext _context;
        
        public IGenericRepository<EthBlock> EthBlocks { get; }
        public IGenericRepository<DashBlock> DashBlocks { get; }
        public IGenericRepository<BtcBlock> BtcBlocks { get; }
        public IGenericRepository<LtcBlock> LtcBlocks { get; }
        public IGenericRepository<BitCoinBlock> BitCoinBlocks { get; }
        public IGenericRepository<ApiAuditLog> ApiAuditLogs { get; }

        // Custom Repository
        public IDashBlockRepository DashBlockRepository { get; }

        public UnitOfWork(BlockCypherDbContext context)
        {
            _context = context;
            EthBlocks = new GenericRepository<EthBlock>(_context);
            DashBlocks = new GenericRepository<DashBlock>(_context);
            BtcBlocks = new GenericRepository<BtcBlock>(_context);
            LtcBlocks = new GenericRepository<LtcBlock>(_context);
            BitCoinBlocks = new GenericRepository<BitCoinBlock>(_context);
            ApiAuditLogs = new GenericRepository<ApiAuditLog>(_context);
            DashBlockRepository = new DashBlockRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
