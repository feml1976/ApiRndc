namespace ApiRndc.Domain.Entities;

/// <summary>
/// Representa un manifiesto de carga registrado en el RNDC
/// </summary>
public class Manifiesto : BaseEntity
{
    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;

    /// <summary>
    /// Número del manifiesto de carga
    /// </summary>
    public string NumManifiestoCarga { get; set; } = string.Empty;

    /// <summary>
    /// Consecutivo de información de viaje
    /// </summary>
    public string? ConsecutivoInformacionViaje { get; set; }

    /// <summary>
    /// Número de manifiesto de transbordo
    /// </summary>
    public string? ManNroManifiestoTransbordo { get; set; }

    /// <summary>
    /// Código de operación de transporte
    /// </summary>
    public string? CodOperacionTransporte { get; set; }

    /// <summary>
    /// Fecha de expedición del manifiesto
    /// </summary>
    public DateTime? FechaExpedicionManifiesto { get; set; }

    /// <summary>
    /// Código municipio de origen del manifiesto
    /// </summary>
    public string? CodMunicipioOrigenManifiesto { get; set; }

    /// <summary>
    /// Código municipio de destino del manifiesto
    /// </summary>
    public string? CodMunicipioDestinoManifiesto { get; set; }

    /// <summary>
    /// Código tipo de identificación del titular del manifiesto
    /// </summary>
    public string? CodIdTitularManifiesto { get; set; }

    /// <summary>
    /// Número de identificación del titular del manifiesto
    /// </summary>
    public string? NumIdTitularManifiesto { get; set; }

    /// <summary>
    /// Placa del vehículo
    /// </summary>
    public string? NumPlaca { get; set; }

    /// <summary>
    /// Placa del remolque
    /// </summary>
    public string? NumPlacaRemolque { get; set; }

    /// <summary>
    /// Código tipo de identificación del conductor
    /// </summary>
    public string? CodIdConductor { get; set; }

    /// <summary>
    /// Número de identificación del conductor
    /// </summary>
    public string? NumIdConductor { get; set; }

    /// <summary>
    /// Código tipo de identificación del conductor 2
    /// </summary>
    public string? CodIdConductor2 { get; set; }

    /// <summary>
    /// Número de identificación del conductor 2
    /// </summary>
    public string? NumIdConductor2 { get; set; }

    /// <summary>
    /// Valor del flete pactado para el viaje
    /// </summary>
    public decimal? ValorFletePactadoViaje { get; set; }

    /// <summary>
    /// Porcentaje de retención en la fuente
    /// </summary>
    public decimal? RetencionFuenteManifiesto { get; set; }

    /// <summary>
    /// Porcentaje de retención ICA
    /// </summary>
    public decimal? RetencionIcaManifiestoCarga { get; set; }

    /// <summary>
    /// Valor del anticipo del manifiesto
    /// </summary>
    public decimal? ValorAnticipoManifiesto { get; set; }

    /// <summary>
    /// Código municipio de pago de saldo
    /// </summary>
    public string? CodMunicipioPagoSaldo { get; set; }

    /// <summary>
    /// Código responsable de pago de cargue (R=Remitente, E=Empresa, D=Destinatario)
    /// </summary>
    public string? CodResponsablePagoCargue { get; set; }

    /// <summary>
    /// Código responsable de pago de descargue (R=Remitente, E=Empresa, D=Destinatario)
    /// </summary>
    public string? CodResponsablePagoDescargue { get; set; }

    /// <summary>
    /// Fecha de pago de saldo del manifiesto
    /// </summary>
    public DateTime? FechaPagoSaldoManifiesto { get; set; }

    /// <summary>
    /// NIT de empresa de monitoreo de flota
    /// </summary>
    public string? NitMonitoreoFlota { get; set; }

    /// <summary>
    /// Aceptación electrónica (S/N)
    /// </summary>
    public string? AceptacionElectronica { get; set; }

    /// <summary>
    /// Observaciones del manifiesto
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Remesas asociadas al manifiesto (consecutivos separados por comas)
    /// </summary>
    public string? RemesasAsociadas { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) del RNDC
    /// </summary>
    public string? IngresoId { get; set; }

    /// <summary>
    /// Relación con las remesas asociadas
    /// </summary>
    public virtual ICollection<ManifiestoRemesa>? ManifiestoRemesas { get; set; }
}
