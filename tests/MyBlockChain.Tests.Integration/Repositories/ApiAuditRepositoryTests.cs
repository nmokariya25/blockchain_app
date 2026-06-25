using Microsoft.EntityFrameworkCore;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.Repositories;

namespace MyBlockChain.Tests.Integration.Repositories
{
    public class ApiAuditRepositoryTests
    {
        private readonly BlockCypherDbContext _context;
        private readonly ApiAuditRepository _repository;

        public ApiAuditRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            _context = new BlockCypherDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            _repository = new ApiAuditRepository(_context);
        }

        [Fact]
        public async Task FetchAllLatestAsync_Returns_Audit_Logs_Ordered_By_RequestDate_Descending()
        {
            _context.ApiAudits.AddRange(
                CreateAuditLog("/api/test1", DateTime.UtcNow.AddMinutes(-10)),
                CreateAuditLog("/api/test2", DateTime.UtcNow.AddMinutes(-5)),
                CreateAuditLog("/api/test3", DateTime.UtcNow));
            await _context.SaveChangesAsync();

            var results = (await _repository.FetchAllLatestAsync()).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal("/api/test3", results[0].Path);
            Assert.Equal("/api/test2", results[1].Path);
            Assert.Equal("/api/test1", results[2].Path);
        }

        private static ApiAuditLog CreateAuditLog(string path, DateTime requestDate) => new()
        {
            HttpMethod = "GET",
            Path = path,
            QueryString = string.Empty,
            RequestBody = string.Empty,
            Scheme = "http",
            IsHttps = false,
            StatusCode = 200,
            RequestDate = requestDate,
            ResponseDate = requestDate
        };
    }
}
