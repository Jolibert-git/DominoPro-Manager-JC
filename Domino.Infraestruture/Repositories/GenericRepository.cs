using Domino.Domain.Core;
using Domino.Infraestructure.Contracts;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Domino.Infraestructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : HasId
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync([id]);
        }
         

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
            

        public async Task<List<T>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbSet.AsNoTracking()
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();
        }
            

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }
           

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
            

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
            

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
            

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
            

        public void RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
           

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
            
    }
}
