using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
