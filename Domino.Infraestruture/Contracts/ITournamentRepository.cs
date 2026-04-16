using Domino.Domain.Entities;
using Domino.Infraestructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Infraestructure.Contracs
{
    public interface ITournamentRepository: IGenericRepository<Tournament>
    {
        Task<Tournament?> GetWithRoundsAsync(int tournamentId );
        Task<Tournament?> GetWithRegistrationsAsync(int tournamentId );
        Task<Tournament?> GetFullAsync(int tournamentId );
        Task<List<Tournament>> GetByStatusAsync(TournamentStatus status );
        Task<List<Tournament>> GetActiveAsync();
    }
}
