using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;

namespace Domino.Infraestructure.Contracs
{
    public interface IClassificationRepository : IGenericRepository<Classification>
    {
        Task<List<Classification>> GetByTournamentAsync(int tournamentId );
        Task<Classification?> GetByPlayerAndTournamentAsync(int playerId, int tournamentId);
        Task<List<Classification>> GetFinalStandingsAsync(int tournamentId);
        Task<List<Classification>> GetTopNAsync(int tournamentId, int n);
    }
}
