using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.API.Handlers
{

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;



        public  async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred : {Message}", exception.Message);


            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = exception.Message,
                Type = exception.GetType().Name
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            try
            {
                // Ensure that the response is not already closed before writing
                if (!httpContext.Response.HasStarted)
                {
                    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Response has already started, unable to write to it.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing the response.");
            }

            return true;
        }

    }
}
