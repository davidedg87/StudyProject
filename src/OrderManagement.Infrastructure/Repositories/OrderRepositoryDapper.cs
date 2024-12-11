using Dapper;
using OrderManagement.Common.Models;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Interfaces;
using System.Data;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderRepositoryDapper : IOrderRepository
    {
        private readonly IDbConnection _dbConnection;

        public OrderRepositoryDapper(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task AddAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBulkAsync(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Orders WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Order>(query, new { Id = id });
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var query = "SELECT * FROM Orders WHERE Status = @Status";
            return await _dbConnection.QueryAsync<Order>(query, new { Status = status });
        }

        public Task<Result<int>> GetOrdersCount()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Order> Query(bool track = false)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteBulkAsync(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        Task<Result<IEnumerable<Order>>> IOrderRepository.GetOrdersByStatusAsync(OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
