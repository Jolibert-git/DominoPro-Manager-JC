using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Repositories
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        public RoundRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<List<Round>> GetByTournamentAsync(int tournamentId)
        {
            return await _dbSet.Where(r => r.TournamentId == tournamentId)
                               .OrderBy(r => r.RoundNumber)
                               .ToListAsync();
        }

        public async Task<Round?> GetWithTablesAsync(int roundId)
        {
            return await _dbSet
                    .Include(r => r.Tables)
                    .ThenInclude(m => m.Results)
                    .ThenInclude(res => res.Player)
                    .FirstOrDefaultAsync(r => r.Id == roundId);
        }

        public async Task<Round?> GetCurrentAsync(int tournamentId)
        {
            return await _dbSet
                    .Include(r => r.Tables)
                    .Where(r => r.TournamentId == tournamentId &&
                                r.Status == RoundStatus.InPlay)
                    .OrderByDescending(r => r.RoundNumber)
                    .FirstOrDefaultAsync();
        }

        public async Task<List<Round>> GetByStatusAsync(int tournamentId, RoundStatus status)
        {
            return await _dbSet.Where(r => r.TournamentId == tournamentId && r.Status == status)
                               .OrderBy(r => r.RoundNumber)
                               .ToListAsync();
        }

        
    }
}
