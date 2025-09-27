using Microsoft.EntityFrameworkCore;
using MyBlockchain.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = MyBlockchain.Domain.Entities;

namespace MyBlockchain.Infrastructure.Repositories.Product
{
    public class ProductRepository : GenericRepository<Entity.Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Entity.Product>> GetTopExpensiveProducts(int count)
        {
            return await _context.Products
                .OrderByDescending(p => (double)p.Price)
                .Take(count)
                .ToListAsync();
        }
    }

}
