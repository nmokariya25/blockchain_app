using MyBlockchain.Application.DTOs;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IBtcBlockService
    {
        Task<IEnumerable<BtcBlock>> GetAllAsync();
        Task<BtcBlock> GetByIdAsync(int id);
        Task<BtcBlock> AddAsync(BtcBlock btcBlock);
        Task UpdateAsync(BtcBlock btcBlock);
        Task DeleteAsync(int id);
        Task<BtcBlockDto> GetLatestBlockAsync();
    }

}
