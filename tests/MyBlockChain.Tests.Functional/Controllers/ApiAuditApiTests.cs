using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using MyBlockChain.Tests.Functional.TestFixtures;
using System.Net;
using System.Net.Http.Json;

namespace MyBlockChain.Tests.Functional.Controllers
{
    public class ApiAuditApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ApiAuditApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ApiAudit_History_Returns_Logs_In_Descending_Order()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlockCypherDbContext>();
            db.ApiAudits.AddRange(
                new ApiAuditLog
                {
                    HttpMethod = "GET",
                    Path = "/api/EthBlock/history",
                    QueryString = string.Empty,
                    RequestBody = string.Empty,
                    Scheme = "http",
                    IsHttps = false,
                    StatusCode = 200,
                    RequestDate = DateTime.UtcNow.AddMinutes(-5),
                    ResponseDate = DateTime.UtcNow.AddMinutes(-5)
                },
                new ApiAuditLog
                {
                    HttpMethod = "POST",
                    Path = "/api/EthBlock/fetch",
                    QueryString = string.Empty,
                    RequestBody = string.Empty,
                    Scheme = "https",
                    IsHttps = true,
                    StatusCode = 200,
                    RequestDate = DateTime.UtcNow,
                    ResponseDate = DateTime.UtcNow
                });
            await db.SaveChangesAsync();

            var response = await _client.GetAsync("/api/ApiAudit/history");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var logs = await response.Content.ReadFromJsonAsync<List<ApiAuditLog>>();
            Assert.NotNull(logs);
            Assert.Equal(2, logs.Count);
            Assert.Equal("/api/EthBlock/fetch", logs[0].Path);
            Assert.True(logs[0].IsHttps);
        }
    }
}
