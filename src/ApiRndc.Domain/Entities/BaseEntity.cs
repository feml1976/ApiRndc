namespace ApiRndc.Domain.Entities;

/// <summary>
/// Clase base para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Usuario que creó el registro
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Fecha de última modificación del registro
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Usuario que modificó el registro por última vez
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Indica si el registro está activo
    /// </summary>
    public bool IsActive { get; set; } = true;

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}
