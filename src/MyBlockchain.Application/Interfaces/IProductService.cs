using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IList<Product>> AddProductsInBulk(IList<Product> products);
        Task<IEnumerable<Product>> GetTopExpensiveProductsAsync(int count);
    }
}
