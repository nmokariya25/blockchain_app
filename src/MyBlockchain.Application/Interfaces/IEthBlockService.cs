using MyBlockchain.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IEthBlockService
    {
        Task<EthBlockDto> GetLatestBlockAsync();
    }

}
