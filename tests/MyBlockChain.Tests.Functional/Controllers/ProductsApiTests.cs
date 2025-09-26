using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyBlockchain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Functional.Controllers
{
    public class ProductsApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:5218")
            });
        }

        [Fact]
        public async Task Get_Products_Returns_OK()
        {
            var response = await _client.GetAsync("/api/products");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_Product_Then_GetById()
        {
            var newProduct = new Product { Name = "API Test", Price = 12.34M };
            var postResponse = await _client.PostAsJsonAsync("/api/products", newProduct);

            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            var created = await postResponse.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(created);

            var getResponse = await _client.GetAsync($"/api/products/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var fetched = await getResponse.Content.ReadFromJsonAsync<Product>();
            Assert.Equal("API Test", fetched.Name);
        }
    }
}
