using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Domino.Infraestructure.Repositories
{
    public class PlayerRepository:GenericRepository<Player>, IPlayerRepository
{
        public PlayerRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async Task<Player?> GetByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking()
                               .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Player?> GetWithStatsAsync(int playerId)
        {
            return await _dbSet
                        .Include(p => p.Results)
                        .Include(p => p.Classifications)
                        .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task<List<Player>> GetActivePlayersAsync()
        {
            return await _dbSet.AsNoTracking()
                               .Where(p => p.IsActive)
                               .ToListAsync();
        }

        public async Task<List<Player>> GetByEloRangeAsync(short min, short max)
        {
            return await _dbSet.AsNoTracking()
                               .Where(p => p.Elo >= min && p.Elo <= max && p.IsActive)
                               .OrderByDescending(p => p.Elo)
                               .ToListAsync();
        }

        
        
    }

}
