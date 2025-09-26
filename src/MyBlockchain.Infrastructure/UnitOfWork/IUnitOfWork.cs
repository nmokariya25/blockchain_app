using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Repositories;
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

        // Add more repositories as needed
        // IGenericRepository<Category> Categories { get; }

        Task<int> CompleteAsync();
    }
}
