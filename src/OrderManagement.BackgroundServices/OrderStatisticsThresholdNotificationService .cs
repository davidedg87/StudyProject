using MassTransit;
using OrderManagement.Core.Dto;

namespace OrderManagement.BackgroundServices
{
    //CONSUMER CHE SCATTA A SEGUITO RICEZIONE EVENTO SU RABBITMQ ( NELLO SPECIFICO L'EVENTO VIENE GENERATO DA ORDERSTATISTICSMONITORINGSERVICE )
    public class OrderStatisticsThresholdNotificationService : BackgroundService
    {
        private readonly IBus _bus;
        private readonly ILogger<OrderStatisticsThresholdNotificationService> _logger;

        public OrderStatisticsThresholdNotificationService(IBus bus, ILogger<OrderStatisticsThresholdNotificationService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus.ConnectReceiveEndpoint("order-threshold-events", cfg =>
            {
                cfg.Handler<OrderStatisticsThresholdReachedDto>(context =>
                {
                    _logger.LogInformation("Threshold reached with {OrderCount} order statistics count at {Timestamp}",
                                           context.Message.OrderStatisticsCount, context.Message.Timestamp);
                    // Logica aggiuntiva, ad esempio invio di una notifica
                    return Task.CompletedTask;
                });
            });
            return Task.CompletedTask;
        }
    }


}
