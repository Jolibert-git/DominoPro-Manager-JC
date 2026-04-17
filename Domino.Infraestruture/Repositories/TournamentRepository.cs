using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Repositories
{
    public class TournamentRepository : GenericRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Tournament?> GetWithRoundsAsync(int tournamentId)
        {
            return await _dbSet
                    .Include(t => t.Rondas)
                        .ThenInclude(r => r.Tables)
                    .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }

        public async Task<Tournament?> GetWithRegistrationsAsync(int tournamentId)
        {
            return await _dbSet
                    .Include(t => t.Registrations)
                        .ThenInclude(r => r.Player)
                    .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }

        public async Task<Tournament?> GetFullAsync(int tournamentId)
        {
            return await _dbSet
                    .Include(t => t.Rondas)
                        .ThenInclude(r => r.Tables)
                            .ThenInclude(m => m.Results)
                    .Include(t => t.Registrations)
                        .ThenInclude(r => r.Player)
                    .Include(t => t.Classificationes)
                        .ThenInclude(c => c.Player)
                    .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }

        public async Task<List<Tournament>> GetByStatusAsync(TournamentStatus status)
        {
            return await _dbSet.AsNoTracking()
                               .Where(t => t.Status == status)
                               .OrderByDescending(t => t.StartDate)
                               .ToListAsync();
        }

        public async Task<List<Tournament>> GetActiveAsync()
        {
            return await _dbSet.AsNoTracking()
                               .Where(t => t.Status == TournamentStatus.InCourse ||
                                           t.Status == TournamentStatus.Programmed)
                               .OrderBy(t => t.StartDate)
                               .ToListAsync();
        }

        
    }

}
