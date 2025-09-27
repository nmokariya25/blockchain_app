using Microsoft.AspNetCore.Mvc.Testing;
using MyBlockChain.Tests.Functional.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Functional.Controllers
{
    public class EthBlockApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public EthBlockApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost:5218")
                });
        }

        [Fact]
        public async Task Get_EthBlocks_Returns_OK()
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/EthBlock/save-latest-block", content);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Code: {response.StatusCode}");
            Console.WriteLine($"Response Body: {body}");

            // Assert   
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        }
    }
}
