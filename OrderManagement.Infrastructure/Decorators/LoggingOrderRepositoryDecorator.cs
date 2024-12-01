using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Infrastructure.Decorators
{
    public class LoggingOrderRepositoryDecorator : LoggingRepositoryDecorator<Order>, IOrderRepository
    {
        private readonly IOrderRepository _inner;

        public LoggingOrderRepositoryDecorator(IOrderRepository inner) : base(inner)
        {
            _inner = inner;
        }

        public async Task<Result<int>> GetOrdersCount()
        {
            Console.WriteLine("Logging: Getting orders count");
            return await _inner.GetOrdersCount();
        }

        public async Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            Console.WriteLine($"Logging: Getting orders with status {status}");
            return await _inner.GetOrdersByStatusAsync(status);
        }

        public Task SoftDeleteBulkAsync(List<int> ids)
        {
            return _inner.SoftDeleteBulkAsync(ids);
        }
    }
}