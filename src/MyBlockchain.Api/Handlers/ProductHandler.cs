using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Services;
using MyBlockchain.Domain.Entities;

namespace MyBlockchain.Api.Handlers
{
    public class ProductHandler
    {
        private readonly IProductService _productService;

        public ProductHandler(IProductService productService)
        {
            this._productService = productService;
        }

        public async Task<int> Handle(Product product)
        {
            await  _productService.AddAsync(product);
            return product.Id;
        }
    }
}
