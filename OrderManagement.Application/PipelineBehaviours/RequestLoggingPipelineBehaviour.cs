using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Models;

namespace OrderManagement.Application.PipelineBehaviours
{
    public class RequestLoggingPipelineBehavior<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;

            _logger.LogInformation(
                "Processing request {RequestName}", requestName);

            TResponse result = await next();

            // Gestisci il risultato, verificando se è di tipo Result<T> o Unit
            if (result is IResult resultWithStatus)
            {
                // Se il risultato è un Result<T>, possiamo verificare se è successo o ha errori
                if (resultWithStatus.Success)
                {
                    _logger.LogInformation("Completed request {RequestName} successfully", requestName);
                }
                else
                {
                    _logger.LogError("Completed request {RequestName} with error(s): {Errors}", requestName, string.Join(", ", resultWithStatus.Errors));
                }
            }
            else
            {
                // Se il risultato è di tipo diverso, possiamo loggare solo il tipo di risposta
                _logger.LogInformation("Completed request {RequestName} with response of type {ResponseType}", requestName, typeof(TResponse).Name);
            }

            return result;
        }
    }
}
