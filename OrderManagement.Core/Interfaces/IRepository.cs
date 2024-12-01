namespace OrderManagement.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IQueryable<T> Query(bool track = false);
        Task DeleteBulkAsync(List<int> ids);
    }
}
