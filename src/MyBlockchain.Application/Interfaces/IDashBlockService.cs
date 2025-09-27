using MyBlockchain.Application.DTOs;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IDashBlockService
    {
        Task<IEnumerable<DashBlock>> GetAllAsync();
        Task<DashBlock> GetByIdAsync(int id);
        Task<DashBlock> AddAsync(DashBlock ethBlock);
        Task UpdateAsync(DashBlock ethBlock);
        Task DeleteAsync(int id);
        Task<DashBlockDto> GetLatestBlockAsync();
        Task<IEnumerable<DashBlock>> FetchAllLatestAsync(int count = 0);
    }
}
