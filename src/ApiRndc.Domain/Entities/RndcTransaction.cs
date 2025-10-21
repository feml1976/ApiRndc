using ApiRndc.Domain.Enums;

namespace ApiRndc.Domain.Entities;

/// <summary>
/// Representa una transacción enviada al RNDC
/// </summary>
public class RndcTransaction : BaseEntity
{
    /// <summary>
    /// Tipo de transacción (proceso RNDC)
    /// </summary>
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Estado actual de la transacción
    /// </summary>
    public TransactionStatus Status { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) devuelto por el RNDC
    /// </summary>
    public string? IngresoId { get; set; }

    /// <summary>
    /// XML enviado al RNDC
    /// </summary>
    public string RequestXml { get; set; } = string.Empty;

    /// <summary>
    /// XML de respuesta del RNDC
    /// </summary>
    public string? ResponseXml { get; set; }

    /// <summary>
    /// Mensaje de error en caso de fallo
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Código de error del RNDC
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Número de intentos realizados
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Fecha del último intento
    /// </summary>
    public DateTime? LastRetryAt { get; set; }

    /// <summary>
    /// Fecha de envío exitoso
    /// </summary>
    public DateTime? SuccessAt { get; set; }

    /// <summary>
    /// Datos adicionales en formato JSON
    /// </summary>
    public string? AdditionalData { get; set; }

    /// <summary>
    /// Referencia externa (consecutivo de remesa, manifiesto, etc.)
    /// </summary>
    public string? ExternalReference { get; set; }

    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NitEmpresaTransporte { get; set; } = string.Empty;
}
