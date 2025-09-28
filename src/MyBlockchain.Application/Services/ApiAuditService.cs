using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Services
{
    public class ApiAuditService : IApiAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiAuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiAuditLog> AddAsync(ApiAuditLog apiAuditLog)
        {
            await _unitOfWork.ApiAuditLogs.AddAsync(apiAuditLog);
            await _unitOfWork.CompleteAsync();
            return apiAuditLog;
        }
    }
}
