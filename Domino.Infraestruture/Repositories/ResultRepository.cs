using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Domino.Infraestructure.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        public ResultRepository(ApplicationDbContext context) 
            : base(context) 
        { 
        }

        public async Task<List<Result>> GetByTableAsync(int tableId)
        {
            return await _dbSet.Include(r => r.Player)
                               .Include(r => r.Playmate)
                               .Where(r => r.TableId == tableId)
                               .ToListAsync();
        }

        public async Task<List<Result>> GetByPlayerAsync(int playerId)
        {
            return await _dbSet.Include(r => r.Table)
                               .Where(r => r.PlayerId == playerId || r.PlaymateId == playerId)
                               .OrderByDescending(r => r.Id)
                               .ToListAsync();
        }

        public async Task<List<Result>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId)
        {
            return await _dbSet.Include(r => r.Table)
                               .ThenInclude(m => m.Round)
                               .Where(r => (r.PlayerId == playerId || r.PlaymateId == playerId) &&
                                            r.Table.Round.TournamentId == tournamentId)
                               .ToListAsync();
        }

        public async Task<Result?> GetWinnerOfTableAsync(int tableId)
        {
            return await _dbSet.Include(r => r.Player)
                               .FirstOrDefaultAsync(r => r.TableId == tableId && r.IsWinner);
        }

        
    }
}
