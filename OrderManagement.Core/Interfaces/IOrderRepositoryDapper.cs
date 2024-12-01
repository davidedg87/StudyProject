using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;

namespace OrderManagement.Core.Interfaces
{
    public interface IOrderRepositoryDapper
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }
}
