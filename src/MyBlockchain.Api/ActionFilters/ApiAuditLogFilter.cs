using Microsoft.AspNetCore.Mvc.Filters;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
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
            request.EnableBuffering();
            var requestBody = "";
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            var auditLog = new ApiAuditLog
            {
                HttpMethod = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                RequestBody = requestBody,
                RequestDate = DateTime.UtcNow
            };

            // Proceed to the action
            var executedContext = await next();

            var response = context.HttpContext.Response;
            auditLog.StatusCode = response.StatusCode;
            auditLog.ResponseDate = DateTime.UtcNow;
            await _apiAuditService.AddAsync(auditLog);
        }
    }

}
