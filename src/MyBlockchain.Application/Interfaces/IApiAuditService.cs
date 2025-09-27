using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IApiAuditService
    {
        Task<ApiAuditLog> AddAsync(ApiAuditLog apiAuditLog);
    }
}
