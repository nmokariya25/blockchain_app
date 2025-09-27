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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetByIdAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IList<Product>> AddProductsInBulk(IList<Product> products)
        {
            foreach (var product in products)
            {
                await _unitOfWork.Products.AddAsync(product);
            }
            await _unitOfWork.CompleteAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetTopExpensiveProductsAsync(int count)
        {
            var products = await _unitOfWork.ProductRepository.GetTopExpensiveProducts(count);
            return products;
        }
    }
}
