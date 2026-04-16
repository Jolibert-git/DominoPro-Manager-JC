using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Infraestructure.Contracs
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IPlayerRepository Players { get; }
        ITournamentRepository Tournaments { get; }
        ITournamentRegistrationRepository Registrations { get; }
        IRoundRepository Rounds { get; }
        ITableRepository Tables { get; }
        IResultRepository Results { get; }
        IClassificationRepository Classifications { get; }


        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
