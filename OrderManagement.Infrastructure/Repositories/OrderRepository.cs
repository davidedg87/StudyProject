using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly IMemoryCache _cache;

        public OrderRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<Result<int>> GetOrdersCount()
        {
            var count =  await Query()
                .CountAsync();

            return Result<int>.SuccessResult(count);
        }


        //USING LINQ QUERY ON EF
        public async Task<Result<IEnumerable<Order>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders =  await Query()
                .Where(o => o.Status == status)
                .ToListAsync();

            return Result<IEnumerable<Order>>.SuccessResult(orders);
        }

        //USING SQL RAW
        public async Task<Result<IEnumerable<Order>>> GetOrdersByRawSqlAsync(OrderStatus status)
        {
            var orders =  await _context.Orders
                .FromSqlRaw("SELECT * FROM Orders WHERE Status = {0}", status)
                .ToListAsync();

            return Result<IEnumerable<Order>>.SuccessResult(orders);
        }

        //UING MIXED QUERY WITH CACHE IN IMEMORYCACHE
        public async Task<Result<IEnumerable<Order>>?> GetFilteredOrdersAsync(OrderStatus status, string customerName)
        {
            // Creiamo una chiave unica per la cache
            var cacheKey = $"Orders_{status}_{customerName}";

            // Controlliamo se i dati sono già presenti nella cache
            if (!_cache.TryGetValue(cacheKey, out List<Order>? filteredOrders))
            {
                // Se non ci sono dati in cache, eseguiamo la query
                var sql = "SELECT * FROM Orders";
                filteredOrders = await _context.Orders
                    .FromSqlRaw(sql)
                    .Where(o => o.Status == status && o.CustomerName == customerName) // Filtro LINQ
                    .ToListAsync();

                // Memorizziamo i risultati nella cache
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Imposta la scadenza della cache

                _cache.Set(cacheKey, filteredOrders, cacheEntryOptions);
            }

            return Result<IEnumerable<Order>>.SuccessResult(filteredOrders!);
        }


        public async Task SoftDeleteBulkAsync(List<int> ids)
        {
            await _context.Orders
                .Where(o => ids.Contains(o.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(b => b.IsDeleted, true).SetProperty(b => b.DeletedAt, DateTime.UtcNow));
        }

        public async Task InsertBulkAsync(List<Order> orders)
        {
            //PROVARE CON BULK INSERT SE SI RIESCE AD INSTALLARE IL PACKAGE
        }
    }
}
