using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;

namespace OrderManagement.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Result<int>> GetOrdersCount();
        Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status);
        Task SoftDeleteBulkAsync(List<int> ids);
    }
}
