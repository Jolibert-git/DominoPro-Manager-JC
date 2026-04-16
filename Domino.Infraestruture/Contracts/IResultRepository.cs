using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Infraestructure.Contracs
{
    public interface IResultRepository : IGenericRepository<Result>
    {
        Task<List<Result>> GetByTableAsync(int tableId );
        Task<List<Result>> GetByPlayerAsync(int playerId );
        Task<List<Result>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId );
        Task<Result?> GetWinnerOfTableAsync(int tableId );
    }
}
