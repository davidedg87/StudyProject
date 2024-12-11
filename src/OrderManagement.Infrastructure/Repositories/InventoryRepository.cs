using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Infrastructure.Repositories
{
    public class InventoryRepository : Repository<InventoryItem>, IInventoryRepository
    {

        public InventoryRepository(AppDbContext context) : base(context) { }

        public async Task<InventoryItem?> GetByNameAsync(string name)
        {
            return await Query()
                .FirstOrDefaultAsync(i => i.Name == name);
        }


    }

}
