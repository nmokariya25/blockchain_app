using MyBlockchain.Application.DTOs;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface ILtcBlockService
    {
        Task<IEnumerable<LtcBlock>> GetAllAsync();
        Task<LtcBlock> GetByIdAsync(int id);
        Task<LtcBlock> AddAsync(LtcBlock btcBlock);
        Task UpdateAsync(LtcBlock btcBlock);
        Task DeleteAsync(int id);
        Task<LtcBlockDto> GetLatestBlockAsync();
    }
}
