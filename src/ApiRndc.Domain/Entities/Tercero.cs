namespace ApiRndc.Domain.Entities;

/// <summary>
/// Representa un tercero (conductor, propietario, etc.) registrado en el RNDC
/// </summary>
public class Tercero : BaseEntity
{
    /// <summary>
    /// NIT de la empresa de transporte
    /// </summary>
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;

    /// <summary>
    /// Código tipo de identificación del tercero (C, N, E, P)
    /// </summary>
    public string CodTipoIdTercero { get; set; } = string.Empty;

    /// <summary>
    /// Número de identificación del tercero
    /// </summary>
    public string NumIdTercero { get; set; } = string.Empty;

    /// <summary>
    /// Nombre o nombres del tercero
    /// </summary>
    public string NomIdTercero { get; set; } = string.Empty;

    /// <summary>
    /// Primer apellido del tercero
    /// </summary>
    public string? PrimerApellidoIdTercero { get; set; }

    /// <summary>
    /// Segundo apellido del tercero
    /// </summary>
    public string? SegundoApellidoIdTercero { get; set; }

    /// <summary>
    /// Número de teléfono de contacto
    /// </summary>
    public string? NumTelefonoContacto { get; set; }

    /// <summary>
    /// Dirección del tercero
    /// </summary>
    public string? NomenclaturaDireccion { get; set; }

    /// <summary>
    /// Código del municipio según RNDC
    /// </summary>
    public string? CodMunicipioRndc { get; set; }

    /// <summary>
    /// Código de la sede del tercero
    /// </summary>
    public string? CodSedeTercero { get; set; }

    /// <summary>
    /// Nombre de la sede del tercero
    /// </summary>
    public string? NomSedeTercero { get; set; }

    /// <summary>
    /// Número de licencia de conducción
    /// </summary>
    public string? NumLicenciaConduccion { get; set; }

    /// <summary>
    /// Categoría de la licencia de conducción
    /// </summary>
    public string? CodCategoriaLicenciaConduccion { get; set; }

    /// <summary>
    /// Fecha de vencimiento de la licencia
    /// </summary>
    public DateTime? FechaVencimientoLicencia { get; set; }

    /// <summary>
    /// Latitud de la ubicación
    /// </summary>
    public decimal? Latitud { get; set; }

    /// <summary>
    /// Longitud de la ubicación
    /// </summary>
    public decimal? Longitud { get; set; }

    /// <summary>
    /// Indica si pertenece al régimen simple (S/N)
    /// </summary>
    public string? RegimenSimple { get; set; }

    /// <summary>
    /// Número de radicado (IngresoId) del RNDC
    /// </summary>
    public string? IngresoId { get; set; }
}
