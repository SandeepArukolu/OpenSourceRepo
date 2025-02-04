 using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace OpenSourceProj.Modals
{
    public class GlobalExecption : IExceptionHandler
    {
        private readonly ILogger<GlobalExecption> _logger;

        public GlobalExecption(ILogger<GlobalExecption> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var respone = new ErrorResponse
            {
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ContentType = "application/json",
            };

            var JsonResponse= JsonSerializer.Serialize(respone);

            _logger.LogError(JsonResponse);
            await httpContext.Response.WriteAsJsonAsync(JsonResponse, cancellationToken);

            return true;
        }
    }
}
