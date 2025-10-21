using ApiRndc.Domain.Entities;

namespace ApiRndc.Domain.Interfaces;

/// <summary>
/// Patrón Unit of Work para manejar transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositorio de transacciones RNDC
    /// </summary>
    IRepository<RndcTransaction> RndcTransactions { get; }

    /// <summary>
    /// Repositorio de terceros
    /// </summary>
    IRepository<Tercero> Terceros { get; }

    /// <summary>
    /// Repositorio de vehículos
    /// </summary>
    IRepository<Vehiculo> Vehiculos { get; }

    /// <summary>
    /// Repositorio de remesas
    /// </summary>
    IRepository<Remesa> Remesas { get; }

    /// <summary>
    /// Repositorio de manifiestos
    /// </summary>
    IRepository<Manifiesto> Manifiestos { get; }

    /// <summary>
    /// Repositorio de relación manifiesto-remesa
    /// </summary>
    IRepository<ManifiestoRemesa> ManifiestoRemesas { get; }

    /// <summary>
    /// Guarda todos los cambios en la base de datos
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia una transacción
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma una transacción
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Revierte una transacción
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
