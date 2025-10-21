using ApiRndc.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiRndc.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos de la aplicación
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<RndcTransaction> RndcTransactions => Set<RndcTransaction>();
    public DbSet<Tercero> Terceros => Set<Tercero>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<Remesa> Remesas => Set<Remesa>();
    public DbSet<Manifiesto> Manifiestos => Set<Manifiesto>();
    public DbSet<ManifiestoRemesa> ManifiestoRemesas => Set<ManifiestoRemesa>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración de esquema
        builder.HasDefaultSchema("public");

        // Configuración de RndcTransaction
        builder.Entity<RndcTransaction>(entity =>
        {
            entity.ToTable("rndc_transactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TransactionType).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.RequestXml).IsRequired();
            entity.Property(e => e.NitEmpresaTransporte).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IngresoId).HasMaxLength(100);
            entity.Property(e => e.ExternalReference).HasMaxLength(100);
            entity.Property(e => e.ErrorCode).HasMaxLength(50);

            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.TransactionType);
            entity.HasIndex(e => e.IngresoId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Configuración de Tercero
        builder.Entity<Tercero>(entity =>
        {
            entity.ToTable("terceros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumNitEmpresaTransporte).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CodTipoIdTercero).HasMaxLength(10).IsRequired();
            entity.Property(e => e.NumIdTercero).HasMaxLength(50).IsRequired();
            entity.Property(e => e.NomIdTercero).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PrimerApellidoIdTercero).HasMaxLength(100);
            entity.Property(e => e.SegundoApellidoIdTercero).HasMaxLength(100);
            entity.Property(e => e.NumLicenciaConduccion).HasMaxLength(50);
            entity.Property(e => e.IngresoId).HasMaxLength(100);

            entity.HasIndex(e => e.NumIdTercero);
            entity.HasIndex(e => e.IngresoId);
        });

        // Configuración de Vehiculo
        builder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("vehiculos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumPlaca).HasMaxLength(20).IsRequired();
            entity.Property(e => e.NumNitEmpresaTransporte).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IngresoId).HasMaxLength(100);

            entity.HasIndex(e => e.NumPlaca).IsUnique();
            entity.HasIndex(e => e.IngresoId);
        });

        // Configuración de Remesa
        builder.Entity<Remesa>(entity =>
        {
            entity.ToTable("remesas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ConsecutivoRemesa).HasMaxLength(50).IsRequired();
            entity.Property(e => e.NumNitEmpresaTransporte).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IngresoId).HasMaxLength(100);

            entity.HasIndex(e => e.ConsecutivoRemesa);
            entity.HasIndex(e => e.IngresoId);
        });

        // Configuración de Manifiesto
        builder.Entity<Manifiesto>(entity =>
        {
            entity.ToTable("manifiestos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumManifiestoCarga).HasMaxLength(50).IsRequired();
            entity.Property(e => e.NumNitEmpresaTransporte).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IngresoId).HasMaxLength(100);

            entity.HasIndex(e => e.NumManifiestoCarga);
            entity.HasIndex(e => e.IngresoId);
        });

        // Configuración de ManifiestoRemesa
        builder.Entity<ManifiestoRemesa>(entity =>
        {
            entity.ToTable("manifiesto_remesas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ConsecutivoRemesa).HasMaxLength(50).IsRequired();

            entity.HasOne(e => e.Manifiesto)
                .WithMany(m => m.ManifiestoRemesas)
                .HasForeignKey(e => e.ManifiestoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Remesa)
                .WithMany()
                .HasForeignKey(e => e.RemesaId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
