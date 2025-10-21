using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Interfaces;
using ApiRndc.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiRndc.Infrastructure.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<RndcTransaction>? _rndcTransactions;
    private IRepository<Tercero>? _terceros;
    private IRepository<Vehiculo>? _vehiculos;
    private IRepository<Remesa>? _remesas;
    private IRepository<Manifiesto>? _manifiestos;
    private IRepository<ManifiestoRemesa>? _manifiestoRemesas;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<RndcTransaction> RndcTransactions =>
        _rndcTransactions ??= new Repository<RndcTransaction>(_context);

    public IRepository<Tercero> Terceros =>
        _terceros ??= new Repository<Tercero>(_context);

    public IRepository<Vehiculo> Vehiculos =>
        _vehiculos ??= new Repository<Vehiculo>(_context);

    public IRepository<Remesa> Remesas =>
        _remesas ??= new Repository<Remesa>(_context);

    public IRepository<Manifiesto> Manifiestos =>
        _manifiestos ??= new Repository<Manifiesto>(_context);

    public IRepository<ManifiestoRemesa> ManifiestoRemesas =>
        _manifiestoRemesas ??= new Repository<ManifiestoRemesa>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
