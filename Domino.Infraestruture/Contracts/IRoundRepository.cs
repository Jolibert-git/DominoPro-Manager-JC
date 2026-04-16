using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Contracs
{
    public interface IRoundRepository : IGenericRepository<Round>
    {
        Task<List<Round>> GetByTournamentAsync(int tournamentId );
        Task<Round?> GetWithTablesAsync(int roundId );
        Task<Round?> GetCurrentAsync(int tournamentId );
        Task<List<Round>> GetByStatusAsync(int tournamentId, RoundStatus status );
    }
}
