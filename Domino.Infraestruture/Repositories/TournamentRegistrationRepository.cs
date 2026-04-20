using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Repositories
{
    public class TournamentRegistrationRepository : GenericRepository<TournamentRegistration>, ITournamentRegistrationRepository
    {
        public TournamentRegistrationRepository(ApplicationDbContext context) 
            : base(context) 
        {
        }

        public async Task<TournamentRegistration?> GetByPlayerAndTournamentAsync(int playerId, int tournamentId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.PlayerId == playerId && r.TournamentId == tournamentId);
        }

        public async Task<List<TournamentRegistration>> GetByTournamentAsync(int tournamentId)
        {
            return await _dbSet.Include(r => r.Player)
                               .Where(r => r.TournamentId == tournamentId)
                               .OrderBy(r => r.DorsalNumber)
                               .ToListAsync();
        }

        public async Task<List<TournamentRegistration>> GetByPlayerAsync(int playerId)
        {
            return await _dbSet.Include(r => r.Tournament)
                               .Where(r => r.PlayerId == playerId)
                               .OrderByDescending(r => r.RegistrationDate)
                               .ToListAsync();
        }

        public async Task<bool> IsPlayerRegisteredAsync(int playerId, int tournamentId)
        {
            return await _dbSet.AnyAsync(r => r.PlayerId == playerId &&
                                             r.TournamentId == tournamentId &&
                                             r.Status != RegistrationStatus.Withdrawn &&
                                             r.Status != RegistrationStatus.Disqualified);
        }

        public async Task<List<TournamentRegistration>> GetConfirmedAsync(int tournamentId)
        {
            return await _dbSet.Include(r => r.Player)
                               .Where(r => r.TournamentId == tournamentId &&
                                           r.Status == RegistrationStatus.Confirmed)
                               .ToListAsync();
        }

        
    }
}
