using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using MyBlockchain.Api.Controllers;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using MyBlockChain.Tests.Functional.TestFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBlockChain.Tests.Functional.Controllers
{
    public class BtcBlockApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public BtcBlockApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost:5218")
                });
        }

        [Fact]
        public async Task Get_BtcBlocks_Returns_OK()
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/BtcBlock/fetch", content);
            var body = await response.Content.ReadAsStringAsync();

            // Assert   
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Fetch_Save_BtcBlocks_Returns_OK()
        {
            // Arrange
            var mockService = new Mock<IBtcBlockService>();
            var mockLogger = new Mock<ILogger<BtcBlockController>>();

            var fakeBlock = new BtcBlock { Height = 123456 };

            mockService.Setup(s => s.FetchAndSaveAsync())
                       .ReturnsAsync(fakeBlock);

            var controller = new BtcBlockController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.SaveLatestBlock();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);

            Assert.Equal("Block data saved successfully", doc.RootElement.GetProperty("message").GetString());
            Assert.Equal(123456, doc.RootElement.GetProperty("blockHeight").GetInt32());

            mockService.Verify(s => s.FetchAndSaveAsync(), Times.Once);
        }
    }
}
