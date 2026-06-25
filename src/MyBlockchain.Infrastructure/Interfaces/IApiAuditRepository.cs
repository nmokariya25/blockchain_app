using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Interfaces
{
    public interface IApiAuditRepository
    {
        Task<IEnumerable<Entity.ApiAuditLog>> FetchAllLatestAsync(int count = 0);
    }
}
