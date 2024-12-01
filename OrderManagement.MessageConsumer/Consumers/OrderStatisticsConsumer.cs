using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Messaging.Consumer.Consumers
{
    public class OrderStatisticsConsumer : IConsumer<OrderStatistics>
    {
        private readonly IOrderStatisticsRepository _orderStatisticsRepository;
        private readonly ILogger<OrderStatisticsConsumer> _logger;

        // Inietta il contesto esistente

        public OrderStatisticsConsumer(IOrderStatisticsRepository orderStatisticsRepository, ILogger<OrderStatisticsConsumer> logger)
        {
            _orderStatisticsRepository = orderStatisticsRepository;
            _logger = logger;

        }

        public async Task Consume(ConsumeContext<OrderStatistics> context)
        {

            OrderStatistics message = context.Message;
            _logger.LogInformation("Received order statistics : Count= {Count} ", message.Count);

            // Crea una nuova entità dell'ordine
            var orderStatisticsEntity = new OrderStatistics
            {
                Count = message.Count
            };


            await _orderStatisticsRepository.AddAsync(orderStatisticsEntity);

            _logger.LogInformation("Order saved to DB : Id= {Id} , Count= {Count}", orderStatisticsEntity.Id, orderStatisticsEntity.Count);

        }
    }
}

