
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domino.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        public IPlayerRepository? Players { get; }
        public ITournamentRepository? Tournaments { get; }
        public ITournamentRegistrationRepository? Registrations { get; }
        public IRoundRepository? Rounds { get; }
        public ITableRepository? Tables { get; }
        public IResultRepository? Results { get; }
        public IClassificationRepository? Classifications { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IPlayerRepository? players,
                          ITournamentRepository? tournaments,
                          ITournamentRegistrationRepository? registrations,
                          IRoundRepository? rounds,
                          ITableRepository? tables,
                          IResultRepository? results,
                          IClassificationRepository? classifications
                         )
        {
            _context = context;
            Players = players;
            Tournaments = tournaments;
            Registrations = registrations;
            Rounds = rounds;
            Tables = tables;
            Results = results;
            Classifications = classifications;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task BeginTransactionAsync()
        {
            if (_transaction is not null)
            {
                throw new InvalidOperationException("There aren't acive transation");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("There aren't comfi transation");
            }
            
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction is null)
            {
                return;
            }
            await _context.Database.RollbackTransactionAsync();
        }

        
    }
}
    

