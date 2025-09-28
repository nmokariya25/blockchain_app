using MyBlockchain.Application.DTOs;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IEthBlockService
    {
        Task<IEnumerable<EthBlock>> GetAllAsync();
        Task<EthBlock> GetByIdAsync(int id);
        Task<EthBlock> AddAsync(EthBlock ethBlock);
        Task UpdateAsync(EthBlock ethBlock);
        Task DeleteAsync(int id);
        Task<EthBlock> FetchAndSaveAsync();
        Task<IEnumerable<EthBlock>> FetchAllLatestAsync(int count = 0);
    }
}
