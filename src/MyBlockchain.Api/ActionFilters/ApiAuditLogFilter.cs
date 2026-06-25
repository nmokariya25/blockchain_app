using Microsoft.AspNetCore.Mvc.Filters;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using System.Text;

namespace MyBlockchain.Api.ActionFilters
{
    public class ApiAuditLogFilter : IAsyncActionFilter
    {
        private readonly IApiAuditService _apiAuditService;

        public ApiAuditLogFilter(IApiAuditService apiAuditService)
        {
            _apiAuditService = apiAuditService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var isAuditEndpoint = request.Path.StartsWithSegments("/api/ApiAudit");
            var requestDate = DateTime.UtcNow;
            var requestBody = string.Empty;

            if (!isAuditEndpoint)
            {
                request.EnableBuffering();
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            await next();

            if (isAuditEndpoint)
                return;

            var response = context.HttpContext.Response;
            var auditLog = new ApiAuditLog
            {
                HttpMethod = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                RequestBody = requestBody,
                Scheme = request.Scheme,
                IsHttps = request.IsHttps,
                StatusCode = response.StatusCode,
                RequestDate = requestDate,
                ResponseDate = DateTime.UtcNow
            };

            await _apiAuditService.AddAsync(auditLog);
        }
    }
}
