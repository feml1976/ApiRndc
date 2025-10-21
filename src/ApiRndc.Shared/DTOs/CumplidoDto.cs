namespace ApiRndc.Shared.DTOs;

/// <summary>
/// DTO para cumplido de remesa (Proceso 5) o manifiesto (Proceso 6)
/// </summary>
public class CumplidoDto
{
    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;

    /// <summary>
    /// Consecutivo de remesa o número de manifiesto
    /// </summary>
    public string Consecutivo { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora del cumplido
    /// </summary>
    public DateTime FechaCumplido { get; set; }

    /// <summary>
    /// Latitud del lugar donde se cumplió
    /// </summary>
    public decimal? Latitud { get; set; }

    /// <summary>
    /// Longitud del lugar donde se cumplió
    /// </summary>
    public decimal? Longitud { get; set; }

    /// <summary>
    /// Observaciones del cumplido
    /// </summary>
    public string? Observaciones { get; set; }
}
