using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Interfaces
{
    public interface IInventoryRepository : IRepository<InventoryItem>
    {
        Task<InventoryItem?> GetByNameAsync(string name);
    }
}
