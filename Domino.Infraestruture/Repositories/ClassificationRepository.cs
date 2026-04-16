using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Domino.Infraestructure.Repositories
{
     public class ClassificationRepository : GenericRepository<Classification>, IClassificationRepository
    {
        public ClassificationRepository(ApplicationDbContext context) 
            : base(context) 
        {
        }

        public async Task<List<Classification>> GetByTournamentAsync(int tournamentId)
        {
            return await _dbSet.AsNoTracking()
                               .Include(c => c.Player)
                               .Where(c => c.TournamentId == tournamentId)
                               .OrderBy(c => c.Position)
                               .ToListAsync();
        }
               
        
            

        public async Task<Classification? > GetByPlayerAndTournamentAsync(int playerId, int tournamentId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.PlayerId == playerId && 
                                                         c.TournamentId == tournamentId);
        }
            

        public async Task<List<Classification>> GetFinalStandingsAsync(int tournamentId)
        {
            return await _dbSet.AsNoTracking()
                           .Include(c => c.Player)
                           .Where(c => c.TournamentId == tournamentId && c.IsFinal)
                           .OrderBy(c => c.Position)
                           .ToListAsync();
        }
               

        public async Task<List<Classification>> GetTopNAsync(int tournamentId, int n)
        {
            return await _dbSet.AsNoTracking()
                           .Include(c => c.Player)
                           .Where(c => c.TournamentId == tournamentId)
                           .OrderBy(c => c.Position)
                           .Take(n)
                           .ToListAsync();
        }
            
    }
}
