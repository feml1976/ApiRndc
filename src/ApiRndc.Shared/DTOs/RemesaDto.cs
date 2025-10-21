namespace ApiRndc.Shared.DTOs;

/// <summary>
/// DTO para expedición de remesa (Proceso 3)
/// </summary>
public class RemesaDto
{
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;
    public string ConsecutivoRemesa { get; set; } = string.Empty;
    public string? ConsecutivoInformacionCarga { get; set; }
    public string? CodOperacionTransporte { get; set; }
    public string? CodNaturalezaCarga { get; set; }
    public decimal? CantidadCargada { get; set; }
    public string? UnidadMedidaCapacidad { get; set; }
    public string? CodTipoEmpaque { get; set; }
    public decimal? PesoContenedorVacio { get; set; }
    public string? MercanciaRemesa { get; set; }
    public string? DescripcionCortaProducto { get; set; }
    public string? CodTipoIdRemitente { get; set; }
    public string? NumIdRemitente { get; set; }
    public string? CodSedeRemitente { get; set; }
    public string? CodTipoIdDestinatario { get; set; }
    public string? NumIdDestinatario { get; set; }
    public string? CodSedeDestinatario { get; set; }
    public string? DuenoPoliza { get; set; }
    public string? NumPolizaTransporte { get; set; }
    public string? CompaniaSeguro { get; set; }
    public DateTime? FechaVencimientoPolizaCarga { get; set; }
    public int? HorasPactoCarga { get; set; }
    public int? MinutosPactoCarga { get; set; }
    public int? HorasPactoDescargue { get; set; }
    public int? MinutosPactoDescargue { get; set; }
    public string? CodTipoIdPropietario { get; set; }
    public string? NumIdPropietario { get; set; }
    public string? CodSedePropietario { get; set; }
    public string? OrdenServicioGenerador { get; set; }
    public DateTime? FechaCitaPactadaCargue { get; set; }
    public string? HoraCitaPactadaCargue { get; set; }
    public DateTime? FechaCitaPactadaDescargue { get; set; }
    public string? HoraCitaPactadaDescargueRemesa { get; set; }
    public string? PermisoCargaExtra { get; set; }
    public string? NumIdGps { get; set; }
    public string? CodigoUn { get; set; }
    public string? SubpartidaCode { get; set; }
    public string? CodigoArancelCode { get; set; }
    public string? GrupoEmbalajeEnvase { get; set; }
    public string? EstadoMercancia { get; set; }
    public string? UnidadMedidaProducto { get; set; }
    public decimal? CantidadProducto { get; set; }
}
