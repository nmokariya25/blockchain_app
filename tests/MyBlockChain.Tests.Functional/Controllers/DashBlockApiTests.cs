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
    public class DashBlockApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public DashBlockApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost:5218")
                });
        }

        [Fact]
        public async Task Get_DashBlocks_Returns_OK()
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/DashBlock/fetch", content);
            var body = await response.Content.ReadAsStringAsync();
            
            // Assert   
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
