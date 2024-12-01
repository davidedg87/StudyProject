using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Infrastructure.Decorators
{
    public class CachingOrderRepositoryDecorator : CachingRepositoryDecorator<Order>, IOrderRepository
    {
        private readonly IOrderRepository _inner;


        public CachingOrderRepositoryDecorator(IOrderRepository inner) : base(inner)
        {
            _inner = inner;
        }

        public async Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _inner.GetOrdersByStatusAsync(status);
        }

        public async Task<Result<int>> GetOrdersCount()
        {
            return await _inner.GetOrdersCount();
        }

        public Task SoftDeleteBulkAsync(List<int> ids)
        {
            return _inner.SoftDeleteBulkAsync(ids);
        }
    }
}
