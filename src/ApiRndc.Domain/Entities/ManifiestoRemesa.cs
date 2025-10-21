namespace ApiRndc.Domain.Entities;

/// <summary>
/// Tabla de relación entre Manifiestos y Remesas
/// </summary>
public class ManifiestoRemesa : BaseEntity
{
    /// <summary>
    /// ID del manifiesto
    /// </summary>
    public Guid ManifiestoId { get; set; }

    /// <summary>
    /// Manifiesto asociado
    /// </summary>
    public virtual Manifiesto? Manifiesto { get; set; }

    /// <summary>
    /// Consecutivo de la remesa
    /// </summary>
    public string ConsecutivoRemesa { get; set; } = string.Empty;

    /// <summary>
    /// ID de la remesa (opcional, si existe en BD)
    /// </summary>
    public Guid? RemesaId { get; set; }

    /// <summary>
    /// Remesa asociada (opcional)
    /// </summary>
    public virtual Remesa? Remesa { get; set; }
}
