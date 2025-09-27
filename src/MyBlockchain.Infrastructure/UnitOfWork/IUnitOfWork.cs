using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.Repositories.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> Products { get; }

        IProductRepository ProductRepository { get; }

        Task<int> CompleteAsync();
    }
}
