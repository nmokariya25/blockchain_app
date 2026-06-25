using MyBlockChain.Tests.Functional.TestFixtures;
using System.Net;

namespace MyBlockChain.Tests.Functional.Controllers
{
    public class HealthCheckApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HealthCheckApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/health")]
        [InlineData("/healthchecks")]
        public async Task Health_Endpoints_Return_OK(string route)
        {
            var response = await _client.GetAsync(route);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", body);
        }
    }
}
