namespace ApiRndc.Domain.Enums;

/// <summary>
/// Estados posibles de una transacción RNDC
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Transacción pendiente de envío
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Transacción enviada al RNDC exitosamente
    /// </summary>
    Success = 2,

    /// <summary>
    /// Transacción rechazada por el RNDC
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Transacción en proceso de reintento
    /// </summary>
    Retrying = 4,

    /// <summary>
    /// Transacción cancelada
    /// </summary>
    Cancelled = 5
}
