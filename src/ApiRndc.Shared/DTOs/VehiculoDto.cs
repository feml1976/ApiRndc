namespace ApiRndc.Shared.DTOs;

/// <summary>
/// DTO para registro de vehículos (Proceso 12)
/// </summary>
public class VehiculoDto
{
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;
    public string NumPlaca { get; set; } = string.Empty;
    public string CodConfiguracionUnidadCarga { get; set; } = string.Empty;
    public string? CodMarcaVehiculoCarga { get; set; }
    public string? CodLineaVehiculoCarga { get; set; }
    public int? AnoFabricacionVehiculoCarga { get; set; }
    public string? CodTipoIdPropietario { get; set; }
    public string? NumIdPropietario { get; set; }
    public string? CodTipoIdTenedor { get; set; }
    public string? NumIdTenedor { get; set; }
    public string? CodTipoCombustible { get; set; }
    public decimal? PesoVehiculoVacio { get; set; }
    public string? CodColorVehiculoCarga { get; set; }
    public string? CodTipoCarroceria { get; set; }
    public string? NumNitAseguradoraSoat { get; set; }
    public DateTime? FechaVencimientoSoat { get; set; }
    public string? NumSeguroSoat { get; set; }
    public string? UnidadMedidaCapacidad { get; set; }
}
