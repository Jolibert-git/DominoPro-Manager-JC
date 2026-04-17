using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
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
