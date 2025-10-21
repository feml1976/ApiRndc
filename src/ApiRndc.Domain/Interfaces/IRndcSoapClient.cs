using ApiRndc.Domain.Enums;

namespace ApiRndc.Domain.Interfaces;

/// <summary>
/// Interfaz para el cliente SOAP del RNDC
/// </summary>
public interface IRndcSoapClient
{
    /// <summary>
    /// Envía una transacción al RNDC
    /// </summary>
    /// <param name="transactionType">Tipo de transacción (proceso)</param>
    /// <param name="xmlVariables">XML con las variables del proceso</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Respuesta del RNDC</returns>
    Task<RndcResponse> SendTransactionAsync(
        TransactionType transactionType,
        string xmlVariables,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Construye el XML de solicitud completo
    /// </summary>
    /// <param name="transactionType">Tipo de transacción</param>
    /// <param name="xmlVariables">Variables en formato XML</param>
    /// <returns>XML SOAP completo</returns>
    string BuildRequestXml(TransactionType transactionType, string xmlVariables);
}

/// <summary>
/// Respuesta del servicio RNDC
/// </summary>
public class RndcResponse
{
    /// <summary>
    /// Indica si la transacción fue exitosa
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) devuelto por el RNDC
    /// </summary>
    public string? IngresoId { get; set; }

    /// <summary>
    /// Mensaje de respuesta
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Código de error (si aplica)
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// XML completo de respuesta
    /// </summary>
    public string? ResponseXml { get; set; }

    /// <summary>
    /// Detalles adicionales del error
    /// </summary>
    public string? ErrorDetails { get; set; }
}
