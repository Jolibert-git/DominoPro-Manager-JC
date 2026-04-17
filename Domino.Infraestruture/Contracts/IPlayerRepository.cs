using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;

namespace Domino.Infraestructure.Contracs
{
    public interface IPlayerRepository: IGenericRepository<Player>
    {
        Task<Player?> GetByEmailAsync(string email);
        Task<Player?> GetWithStatsAsync(int playerId );
        Task<List<Player>> GetActivePlayersAsync();
        Task<List<Player>> GetByEloRangeAsync(short min, short max );
    }
    
}
