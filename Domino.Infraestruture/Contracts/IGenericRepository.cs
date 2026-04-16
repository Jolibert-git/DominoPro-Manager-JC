using Domino.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
