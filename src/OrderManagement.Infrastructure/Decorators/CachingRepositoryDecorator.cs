using OrderManagement.Core.Interfaces;

namespace OrderManagement.Infrastructure.Decorators
{
    public class CachingRepositoryDecorator<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _inner;
        private readonly Dictionary<int, T> _cache = new();

        public CachingRepositoryDecorator(IRepository<T> inner)
        {
            _inner = inner;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            if (_cache.ContainsKey(id))
            {
                Console.WriteLine("Returning item from cache...");
                return _cache[id];
            }

            var item = await _inner.GetByIdAsync(id);  // Chiamata al successivo nella catena
            _cache[id] = item!;
            return item;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Console.WriteLine($"Caching: Getting all entities of type {typeof(T).Name}");
            return await _inner.GetAllAsync();
        }

        public async Task AddAsync(T entity)
        {
            Console.WriteLine($"Caching: Adding entity of type {typeof(T).Name}");
            await _inner.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            Console.WriteLine($"Caching: Updating entity of type {typeof(T).Name}");
            await _inner.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            Console.WriteLine($"Caching: Deleting entity of type {typeof(T).Name} with ID {id}");
            await _inner.DeleteAsync(id);
        }

        public IQueryable<T> Query(bool track = false)
        {
            Console.WriteLine($"Caching: Querying entities of type {typeof(T).Name}");
            return _inner.Query(track);
        }

        public Task DeleteBulkAsync(List<int> ids)
        {
            return _inner.DeleteBulkAsync(ids);
        }
    }
}
