namespace ApiRndc.Domain.Entities;

/// <summary>
/// Representa una remesa terrestre de carga registrada en el RNDC
/// </summary>
public class Remesa : BaseEntity
{
    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;

    /// <summary>
    /// Consecutivo de la remesa
    /// </summary>
    public string ConsecutivoRemesa { get; set; } = string.Empty;

    /// <summary>
    /// Consecutivo de información de carga
    /// </summary>
    public string? ConsecutivoInformacionCarga { get; set; }

    /// <summary>
    /// Código de operación de transporte
    /// </summary>
    public string? CodOperacionTransporte { get; set; }

    /// <summary>
    /// Código de naturaleza de la carga
    /// </summary>
    public string? CodNaturalezaCarga { get; set; }

    /// <summary>
    /// Cantidad cargada
    /// </summary>
    public decimal? CantidadCargada { get; set; }

    /// <summary>
    /// Unidad de medida de capacidad
    /// </summary>
    public string? UnidadMedidaCapacidad { get; set; }

    /// <summary>
    /// Código tipo de empaque
    /// </summary>
    public string? CodTipoEmpaque { get; set; }

    /// <summary>
    /// Peso del contenedor vacío
    /// </summary>
    public decimal? PesoContenedorVacio { get; set; }

    /// <summary>
    /// Mercancía de la remesa
    /// </summary>
    public string? MercanciaRemesa { get; set; }

    /// <summary>
    /// Descripción corta del producto
    /// </summary>
    public string? DescripcionCortaProducto { get; set; }

    /// <summary>
    /// Código tipo de identificación del remitente
    /// </summary>
    public string? CodTipoIdRemitente { get; set; }

    /// <summary>
    /// Número de identificación del remitente
    /// </summary>
    public string? NumIdRemitente { get; set; }

    /// <summary>
    /// Código sede del remitente
    /// </summary>
    public string? CodSedeRemitente { get; set; }

    /// <summary>
    /// Código tipo de identificación del destinatario
    /// </summary>
    public string? CodTipoIdDestinatario { get; set; }

    /// <summary>
    /// Número de identificación del destinatario
    /// </summary>
    public string? NumIdDestinatario { get; set; }

    /// <summary>
    /// Código sede del destinatario
    /// </summary>
    public string? CodSedeDestinatario { get; set; }

    /// <summary>
    /// Dueño de la póliza (E=Empresa, G=Generador)
    /// </summary>
    public string? DuenoPoliza { get; set; }

    /// <summary>
    /// Número de póliza de transporte
    /// </summary>
    public string? NumPolizaTransporte { get; set; }

    /// <summary>
    /// Compañía de seguro
    /// </summary>
    public string? CompaniaSeguro { get; set; }

    /// <summary>
    /// Fecha de vencimiento de la póliza
    /// </summary>
    public DateTime? FechaVencimientoPolizaCarga { get; set; }

    /// <summary>
    /// Horas pactadas para cargue
    /// </summary>
    public int? HorasPactoCarga { get; set; }

    /// <summary>
    /// Minutos pactados para cargue
    /// </summary>
    public int? MinutosPactoCarga { get; set; }

    /// <summary>
    /// Horas pactadas para descargue
    /// </summary>
    public int? HorasPactoDescargue { get; set; }

    /// <summary>
    /// Minutos pactados para descargue
    /// </summary>
    public int? MinutosPactoDescargue { get; set; }

    /// <summary>
    /// Código tipo de identificación del propietario
    /// </summary>
    public string? CodTipoIdPropietario { get; set; }

    /// <summary>
    /// Número de identificación del propietario
    /// </summary>
    public string? NumIdPropietario { get; set; }

    /// <summary>
    /// Código sede del propietario
    /// </summary>
    public string? CodSedePropietario { get; set; }

    /// <summary>
    /// Orden de servicio del generador
    /// </summary>
    public string? OrdenServicioGenerador { get; set; }

    /// <summary>
    /// Fecha de cita pactada para cargue
    /// </summary>
    public DateTime? FechaCitaPactadaCargue { get; set; }

    /// <summary>
    /// Hora de cita pactada para cargue
    /// </summary>
    public TimeSpan? HoraCitaPactadaCargue { get; set; }

    /// <summary>
    /// Fecha de cita pactada para descargue
    /// </summary>
    public DateTime? FechaCitaPactadaDescargue { get; set; }

    /// <summary>
    /// Hora de cita pactada para descargue
    /// </summary>
    public TimeSpan? HoraCitaPactadaDescargueRemesa { get; set; }

    /// <summary>
    /// Permiso de carga extra (S/N)
    /// </summary>
    public string? PermisoCargaExtra { get; set; }

    /// <summary>
    /// Número de identificación GPS
    /// </summary>
    public string? NumIdGps { get; set; }

    /// <summary>
    /// Código UN
    /// </summary>
    public string? CodigoUn { get; set; }

    /// <summary>
    /// Código de subpartida
    /// </summary>
    public string? SubpartidaCode { get; set; }

    /// <summary>
    /// Código de arancel
    /// </summary>
    public string? CodigoArancelCode { get; set; }

    /// <summary>
    /// Grupo de embalaje/envase
    /// </summary>
    public string? GrupoEmbalajeEnvase { get; set; }

    /// <summary>
    /// Estado de la mercancía
    /// </summary>
    public string? EstadoMercancia { get; set; }

    /// <summary>
    /// Unidad de medida del producto
    /// </summary>
    public string? UnidadMedidaProducto { get; set; }

    /// <summary>
    /// Cantidad de producto
    /// </summary>
    public decimal? CantidadProducto { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) del RNDC
    /// </summary>
    public string? IngresoId { get; set; }
}
