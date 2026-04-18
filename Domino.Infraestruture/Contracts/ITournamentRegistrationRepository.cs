using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;

namespace Domino.Infraestructure.Contracs
{
    public interface ITournamentRegistrationRepository : IGenericRepository<TournamentRegistration>
    {
        Task<TournamentRegistration?> GetByPlayerAndTournamentAsync(int playerId, int tournamentId );
        Task<List<TournamentRegistration>> GetByTournamentAsync(int tournamentId );
        Task<List<TournamentRegistration>> GetByPlayerAsync(int playerId );
        Task<bool> IsPlayerRegisteredAsync(int playerId, int tournamentId );
        Task<List<TournamentRegistration>> GetConfirmedAsync(int tournamentId );
    }
}
