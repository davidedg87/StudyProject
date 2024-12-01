using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace OrderManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IExceptionHandler _exceptionHandler;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IExceptionHandler exceptionHandler)
        {
            _next = next;
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred : {Message}", ex.Message);


                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Type = ex.GetType().Name
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
