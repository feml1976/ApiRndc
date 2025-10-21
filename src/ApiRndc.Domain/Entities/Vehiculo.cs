namespace ApiRndc.Domain.Entities;

/// <summary>
/// Representa un vehículo registrado en el RNDC
/// </summary>
public class Vehiculo : BaseEntity
{
    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;

    /// <summary>
    /// Placa del vehículo
    /// </summary>
    public string NumPlaca { get; set; } = string.Empty;

    /// <summary>
    /// Código de configuración de la unidad de carga
    /// </summary>
    public string CodConfiguracionUnidadCarga { get; set; } = string.Empty;

    /// <summary>
    /// Código de marca del vehículo
    /// </summary>
    public string? CodMarcaVehiculoCarga { get; set; }

    /// <summary>
    /// Código de línea del vehículo
    /// </summary>
    public string? CodLineaVehiculoCarga { get; set; }

    /// <summary>
    /// Año de fabricación del vehículo
    /// </summary>
    public int? AnoFabricacionVehiculoCarga { get; set; }

    /// <summary>
    /// Código tipo de identificación del propietario
    /// </summary>
    public string? CodTipoIdPropietario { get; set; }

    /// <summary>
    /// Número de identificación del propietario
    /// </summary>
    public string? NumIdPropietario { get; set; }

    /// <summary>
    /// Código tipo de identificación del tenedor
    /// </summary>
    public string? CodTipoIdTenedor { get; set; }

    /// <summary>
    /// Número de identificación del tenedor
    /// </summary>
    public string? NumIdTenedor { get; set; }

    /// <summary>
    /// Código tipo de combustible
    /// </summary>
    public string? CodTipoCombustible { get; set; }

    /// <summary>
    /// Peso del vehículo vacío en kilogramos
    /// </summary>
    public decimal? PesoVehiculoVacio { get; set; }

    /// <summary>
    /// Código de color del vehículo
    /// </summary>
    public string? CodColorVehiculoCarga { get; set; }

    /// <summary>
    /// Código tipo de carrocería
    /// </summary>
    public string? CodTipoCarroceria { get; set; }

    /// <summary>
    /// NIT de la aseguradora del SOAT
    /// </summary>
    public string? NumNitAseguradoraSoat { get; set; }

    /// <summary>
    /// Fecha de vencimiento del SOAT
    /// </summary>
    public DateTime? FechaVencimientoSoat { get; set; }

    /// <summary>
    /// Número del seguro SOAT
    /// </summary>
    public string? NumSeguroSoat { get; set; }

    /// <summary>
    /// Unidad de medida de capacidad
    /// </summary>
    public string? UnidadMedidaCapacidad { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) del RNDC
    /// </summary>
    public string? IngresoId { get; set; }
}
