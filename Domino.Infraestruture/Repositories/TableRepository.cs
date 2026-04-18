using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Repositories
{
    public class TableRepository : GenericRepository<Table>, ITableRepository
    {
        public TableRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Table>> GetByRoundAsync(int roundId)
        {
            return await _dbSet.AsNoTracking()
                               .Where(m => m.RoundId == roundId)
                               .OrderBy(m => m.TableNumber)
                               .ToListAsync();
        }

        public async Task<Table?> GetWithResultsAsync(int tableId)
        {
            return await _dbSet
                    .Include(m => m.Results)
                        .ThenInclude(r => r.Player)
                    .Include(m => m.Results)
                        .ThenInclude(r => r.Playmate)
                    .FirstOrDefaultAsync(m => m.Id == tableId);
        }

        public async Task<List<Table>> GetPendingByRoundAsync(int roundId)
        {
            return await _dbSet.AsNoTracking()
                               .Where(m => m.RoundId == roundId &&
                                           m.Status == GameStatus.Pending)
                               .OrderBy(m => m.TableNumber)
                               .ToListAsync();
        }

        
    }

}
