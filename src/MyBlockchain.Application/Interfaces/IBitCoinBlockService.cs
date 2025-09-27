using MyBlockchain.Application.DTOs;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IBitCoinBlockService
    {
        Task<IEnumerable<BitCoinBlock>> GetAllAsync();
        Task<BitCoinBlock> GetByIdAsync(int id);
        Task<BitCoinBlock> AddAsync(BitCoinBlock btcBlock);
        Task UpdateAsync(BitCoinBlock btcBlock);
        Task DeleteAsync(int id);
        Task<BitCoinBlockDto> GetLatestBlockAsync();
    }
}
