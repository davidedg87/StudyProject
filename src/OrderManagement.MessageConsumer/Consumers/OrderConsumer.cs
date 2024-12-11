using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Messaging.Consumer.Consumers
{
    public class OrderConsumer : IConsumer<Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderConsumer> _logger;

        // Inietta il contesto esistente

        public OrderConsumer()
        {

        }

        public OrderConsumer(IOrderRepository orderRepository, ILogger<OrderConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;

        }

        public async Task Consume(ConsumeContext<Order> context)
        {

            Order message = context.Message;
            _logger.LogInformation("Received order : Id= {Id} , Customer= {Customer}", message.Id, message.CustomerName);

            // Crea una nuova entità dell'ordine
            var orderEntity = new Order
            {
                Id = message.Id,
                CustomerName = message.CustomerName,
                // Imposta altre proprietà necessarie
            };


            await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation("Order saved to DB : Id= {Id} , Customer= {Customer}", message.Id, message.CustomerName);

        }
    }
}

