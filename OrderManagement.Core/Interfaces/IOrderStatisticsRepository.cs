using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Interfaces
{
    public interface IOrderStatisticsRepository : IRepository<OrderStatistics>
    {
        IQueryable<OrderStatistics> Query(bool track = false);
    }
}
