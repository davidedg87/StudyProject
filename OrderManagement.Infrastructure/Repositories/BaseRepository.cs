using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data.DbContext;
using Polly;

namespace OrderManagement.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual IQueryable<T> Query(bool track = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (!track)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public virtual async Task DeleteBulkAsync(List<int> ids)
        {
            await _context.Set<T>().Where(o => ids.Contains(o.Id)).ExecuteDeleteAsync();

        }

    }
}
