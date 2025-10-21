namespace ApiRndc.Domain.Enums;

/// <summary>
/// Tipos de transacciones soportadas por el RNDC
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Registro de Terceros - Proceso 11
    /// </summary>
    RegistroTerceros = 11,

    /// <summary>
    /// Registro de Vehículos - Proceso 12
    /// </summary>
    RegistroVehiculos = 12,

    /// <summary>
    /// Expedición de Remesa Terrestre de Carga - Proceso 3
    /// </summary>
    ExpedicionRemesa = 3,

    /// <summary>
    /// Expedición de Manifiesto de Carga - Proceso 4
    /// </summary>
    ExpedicionManifiesto = 4,

    /// <summary>
    /// Cumplido de Remesa Terrestre de Carga - Proceso 5
    /// </summary>
    CumplidoRemesa = 5,

    /// <summary>
    /// Cumplido de Manifiesto de Carga - Proceso 6
    /// </summary>
    CumplidoManifiesto = 6
}
