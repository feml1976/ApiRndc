namespace ApiRndc.Shared.Constants;

/// <summary>
/// Constantes del sistema RNDC
/// </summary>
public static class RndcConstants
{
    /// <summary>
    /// URL del servicio SOAP del RNDC
    /// </summary>
    public const string ServiceUrl = "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices";

    /// <summary>
    /// Namespace SOAP
    /// </summary>
    public const string SoapNamespace = "urn:BPMServicesIntf-IBPMServices";

    /// <summary>
    /// Tipo de solicitud (siempre es 1)
    /// </summary>
    public const string RequestType = "1";

    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public const string DefaultNitEmpresa = "860504882";

    /// <summary>
    /// Número máximo de reintentos para transacciones fallidas
    /// </summary>
    public const int MaxRetryAttempts = 3;

    /// <summary>
    /// Tiempo de espera entre reintentos (en minutos)
    /// </summary>
    public const int RetryDelayMinutes = 5;
}

/// <summary>
/// Tipos de identificación según RNDC
/// </summary>
public static class TipoIdentificacion
{
    public const string Cedula = "C";
    public const string Nit = "N";
    public const string Extranjeria = "E";
    public const string Pasaporte = "P";
}

/// <summary>
/// Códigos de operación de transporte
/// </summary>
public static class OperacionTransporte
{
    public const string Nacional = "1";
    public const string Internacional = "2";
    public const string Transito = "3";
}
