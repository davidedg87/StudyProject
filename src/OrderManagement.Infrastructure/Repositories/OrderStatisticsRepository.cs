using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data.DbContext;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderStatisticsRepository : Repository<OrderStatistics>, IOrderStatisticsRepository
    {

        public OrderStatisticsRepository(AppDbContext context) : base(context) { }

    }
}
