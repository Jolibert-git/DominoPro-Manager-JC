using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using Domino.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new InvalidOperationException("Ya hay una transacción activa.");
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction is null)
                throw new InvalidOperationException("No hay transacción activa para confirmar.");
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction is null) return;
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
                await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
}
}
    

