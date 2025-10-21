namespace ApiRndc.Shared.DTOs;

/// <summary>
/// DTO para expedición de manifiesto (Proceso 4)
/// </summary>
public class ManifiestoDto
{
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;
    public string NumManifiestoCarga { get; set; } = string.Empty;
    public string? ConsecutivoInformacionViaje { get; set; }
    public string? ManNroManifiestoTransbordo { get; set; }
    public string? CodOperacionTransporte { get; set; }
    public DateTime? FechaExpedicionManifiesto { get; set; }
    public string? CodMunicipioOrigenManifiesto { get; set; }
    public string? CodMunicipioDestinoManifiesto { get; set; }
    public string? CodIdTitularManifiesto { get; set; }
    public string? NumIdTitularManifiesto { get; set; }
    public string? NumPlaca { get; set; }
    public string? NumPlacaRemolque { get; set; }
    public string? CodIdConductor { get; set; }
    public string? NumIdConductor { get; set; }
    public string? CodIdConductor2 { get; set; }
    public string? NumIdConductor2 { get; set; }
    public decimal? ValorFletePactadoViaje { get; set; }
    public decimal? RetencionFuenteManifiesto { get; set; }
    public decimal? RetencionIcaManifiestoCarga { get; set; }
    public decimal? ValorAnticipoManifiesto { get; set; }
    public string? CodMunicipioPagoSaldo { get; set; }
    public string? CodResponsablePagoCargue { get; set; }
    public string? CodResponsablePagoDescargue { get; set; }
    public DateTime? FechaPagoSaldoManifiesto { get; set; }
    public string? NitMonitoreoFlota { get; set; }
    public string? AceptacionElectronica { get; set; }
    public string? Observaciones { get; set; }
    public List<string>? ConsecutivosRemesas { get; set; }
}
