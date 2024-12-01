using OrderManagement.Core.Interfaces;

namespace OrderManagement.Infrastructure.Decorators
{
    public class LoggingRepositoryDecorator<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _inner;

        public LoggingRepositoryDecorator(IRepository<T> inner)
        {
            _inner = inner;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            Console.WriteLine($"Logging: Getting entity of type {typeof(T).Name} with ID {id}");
            return await _inner.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Console.WriteLine($"Logging: Getting all entities of type {typeof(T).Name}");
            return await _inner.GetAllAsync();
        }

        public async Task AddAsync(T entity)
        {
            Console.WriteLine($"Logging: Adding entity of type {typeof(T).Name}");
            await _inner.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            Console.WriteLine($"Logging: Updating entity of type {typeof(T).Name}");
            await _inner.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            Console.WriteLine($"Logging: Deleting entity of type {typeof(T).Name} with ID {id}");
            await _inner.DeleteAsync(id);
        }

        public IQueryable<T> Query(bool track = false)
        {
            Console.WriteLine($"Logging: Querying entities of type {typeof(T).Name}");
            return _inner.Query(track);
        }

        public Task DeleteBulkAsync(List<int> ids)
        {
            return _inner.DeleteBulkAsync(ids);
        }
    }
}
