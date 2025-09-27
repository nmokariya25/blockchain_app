using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Api.Handlers;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using System.Collections.Concurrent;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ProductHandler _productHandler;

        public ProductsController(IProductService productService,
            ProductHandler productHandler)
        {
            _productService = productService;
            _productHandler = productHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var created = await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            try
            {
                await _productService.UpdateAsync(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("bulk_products")]
        public async Task<IActionResult> CreateBulk([FromBody] List<Product> products)
        {   
            var newlyAddedProducts = await _productService.AddProductsInBulk(products);
            if (newlyAddedProducts == null || !newlyAddedProducts.Any())
                return BadRequest("No products were added.");

            return Ok(newlyAddedProducts.Select(s => s.Id));
        }

        [HttpGet("top/{count}")]
        public async Task<IActionResult> GetTopExpensive(int count)
        {
            var products = await _productService.GetTopExpensiveProductsAsync(count);
            return Ok(products);
        }
    }
}
