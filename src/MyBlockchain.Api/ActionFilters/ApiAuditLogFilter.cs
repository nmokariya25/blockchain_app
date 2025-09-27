using Microsoft.AspNetCore.Mvc.Filters;
using MyBlockchain.Domain.Entities;
using MyBlockchain.Infrastructure.Data;
using System.Text;

namespace MyBlockchain.Api.ActionFilters
{
    public class ApiAuditLogFilter : IAsyncActionFilter
    {
        private readonly BlockCypherDbContext _dbContext;

        public ApiAuditLogFilter(BlockCypherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            // Read Request Info
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
            _dbContext.ApiAudits.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
    }

}
