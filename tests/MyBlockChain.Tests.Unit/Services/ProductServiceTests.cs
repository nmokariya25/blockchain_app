using Moq;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Repositories;
using MyBlockchain.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Unit.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IGenericRepository<Product>> _mockRepo;

        public ProductServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IGenericRepository<Product>>();
            _mockUow.Setup(u => u.Products).Returns(_mockRepo.Object);
            _service = new ProductService(_mockUow.Object);
        }

        [Fact]
        public async Task AddProduct_ReturnsProduct()
        {
            var product = new Product { Id = 1, Name = "Test", Price = 10 };
            _mockRepo.Setup(r => r.AddAsync(product)).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.AddAsync(product);

            Assert.Equal(product, result);
            _mockRepo.Verify(r => r.AddAsync(product), Times.Once);
        }
    }
}
