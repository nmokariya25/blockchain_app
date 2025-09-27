using Entity = MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Infrastructure.Repositories.Product
{
    public interface IProductRepository : IGenericRepository<Entity.Product>
    {
        Task<IEnumerable<Entity.Product>> GetTopExpensiveProducts(int count);
    }
}
