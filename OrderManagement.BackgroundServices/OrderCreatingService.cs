using Bogus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Dto;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace OrderManagement.BackgroundServices
{
    //CONSUMER CHE SCATTA A TEMPO
    public class OrderCreatingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderCreatingService> _logger;

        public OrderCreatingService(IServiceScopeFactory scopeFactory, IBus bus, IConfiguration configuration, ILogger<OrderCreatingService> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Faker faker = new Faker();
            var delay = _configuration.GetValue<int>("OrderCreationServiceDelay");

            _logger.LogInformation("OrderCreatingService is starting.");
            _logger.LogInformation("OrderCreatingService will run every {Delay} seconds", delay);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken); // Esegui il polling ogni 30 secondi

                using (var scope = _scopeFactory.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var inventoryItemsRepository = scope.ServiceProvider.GetRequiredService<IInventoryRepository>();


                    //RANDOM INVENTORY ITEM 

                    var count = await inventoryItemsRepository.Query().CountAsync();
                    var randomIndex = new Random().Next(0, count); // Genera indice casuale

                    InventoryItem? inventoryItem = await inventoryItemsRepository.Query().Skip(randomIndex).FirstOrDefaultAsync();
                    
                    if( inventoryItem == null)
                    {
                        _logger.LogWarning("No inventory items found");
                        continue;
                    }

                    Order order = new Order
                    {
                        OrderDate = DateTime.UtcNow,
                        CustomerName = faker.Name.FullName(),
                        Status = OrderStatus.Pending
                    };

                    var randomQuantity = new Random().Next(1, 5);

                    order.OrderItems = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            InventoryItemId = inventoryItem.Id,
                            Quantity = randomQuantity,
                            PricePerUnit = inventoryItem.PricePerUnit
                        }
                    };

                    _logger.LogInformation("Adding order {Order}", JsonSerializer.Serialize(order));

                    await orderRepository.AddAsync(order);

                }
            }
        }
    }

}
