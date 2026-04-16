using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Infraestructure.Contracs
{
    public interface ITableRepository : IGenericRepository<Table>
    {
        Task<List<Table>> GetByRoundAsync(int roundId );
        Task<Table?> GetWithResultsAsync(int tableId );
        Task<List<Table>> GetPendingByRoundAsync(int roundId );
    }
}
