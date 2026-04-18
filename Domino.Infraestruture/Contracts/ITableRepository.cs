using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;

namespace Domino.Infraestructure.Contracs
{
    public interface ITableRepository : IGenericRepository<Table>
    {
        Task<List<Table>> GetByRoundAsync(int roundId );
        Task<Table?> GetWithResultsAsync(int tableId );
        Task<List<Table>> GetPendingByRoundAsync(int roundId );
    }
}
