using Domino.Domain.Core;

namespace Domino.Infraestructure.Contracts
{
    public interface IGenericRepository<T> where T : HasId
    {
        Task<T?> GetByIdAsync(int id );
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedAsync(int page, int pageSize );
        Task<int> CountAsync();
        Task AddAsync(T entity  );
        Task AddRangeAsync(List<T> entities );
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(List<T> entities);
        Task<bool> ExistsAsync(int id );
    }
}
