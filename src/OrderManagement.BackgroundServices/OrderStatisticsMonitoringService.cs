using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Dto;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace OrderManagement.BackgroundServices
{
    //CONSUMER CHE SCATTA A TEMPO
    public class OrderStatisticsMonitoringService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderStatisticsMonitoringService> _logger;

        public OrderStatisticsMonitoringService(IServiceScopeFactory scopeFactory, IBus bus, IConfiguration configuration, ILogger<OrderStatisticsMonitoringService> logger)
        {
            _scopeFactory = scopeFactory;
            _bus = bus;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = _configuration.GetValue<int>("OrderStatisticsMonitoringServiceDelay");
            var threshold = _configuration.GetValue<int>("OrderCountThreshold");

            _logger.LogInformation("OrderStatisticsMonitoringService is starting.");
            _logger.LogInformation("OrderStatisticsMonitoringService will run every {Delay} seconds", delay);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken); // Esegui il polling ogni 30 secondi

                using (var scope = _scopeFactory.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var orderStatisticRepository = scope.ServiceProvider.GetRequiredService<IOrderStatisticsRepository>();

                    // Conta gli ordini
                    var orderCount = await orderRepository.Query().CountAsync();
                    _logger.LogInformation("OrderCount : {OrderCount}", orderCount);

                    // Invia il messaggio a RabbitMQ
                    await _bus.Publish(new OrderStatistics { Count = orderCount });

                    var orderStatisticsCount = await orderStatisticRepository.Query().CountAsync();
                    _logger.LogInformation("OrderStatisticsCount : {OrderStatisticsCount}", orderStatisticsCount);
                    // Controlla soglia e pubblica evento OrderThresholdReached
                    if (orderStatisticsCount >= threshold)
                    {
                        _logger.LogInformation("Order count threshold reached: {Threshold}", threshold);
                        await _bus.Publish(new OrderStatisticsThresholdReachedDto
                        {
                            OrderStatisticsCount = orderStatisticsCount,
                            Timestamp = DateTime.UtcNow
                        });
                    }

                }
            }
        }
    }

}
