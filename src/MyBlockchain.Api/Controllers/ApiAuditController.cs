using Microsoft.AspNetCore.Mvc;
using MyBlockchain.Application.Interfaces;

namespace MyBlockchain.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiAuditController : ControllerBase
    {
        private readonly IApiAuditService _apiAuditService;
        private readonly ILogger<ApiAuditController> _logger;

        public ApiAuditController(IApiAuditService apiAuditService, ILogger<ApiAuditController> logger)
        {
            _apiAuditService = apiAuditService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the history of all HTTP/HTTPS requests handled by the API, ordered by request date descending.
        /// </summary>
        [HttpGet("history/{count?}")]
        public async Task<IActionResult> GetRequestHistory(int count = 0)
        {
            var auditLogs = await _apiAuditService.GetHistoryAsync(count);

            if (!auditLogs.Any())
            {
                _logger.LogInformation("No API audit logs found in the database.");
                return NoContent();
            }

            return Ok(auditLogs);
        }
    }
}
