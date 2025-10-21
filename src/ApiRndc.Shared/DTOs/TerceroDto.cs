namespace ApiRndc.Shared.DTOs;

/// <summary>
/// DTO para registro de terceros (Proceso 11)
/// </summary>
public class TerceroDto
{
    public string NumNitEmpresaTransporte { get; set; } = string.Empty;
    public string CodTipoIdTercero { get; set; } = string.Empty;
    public string NumIdTercero { get; set; } = string.Empty;
    public string NomIdTercero { get; set; } = string.Empty;
    public string? PrimerApellidoIdTercero { get; set; }
    public string? SegundoApellidoIdTercero { get; set; }
    public string? NumTelefonoContacto { get; set; }
    public string? NomenclaturaDireccion { get; set; }
    public string? CodMunicipioRndc { get; set; }
    public string? CodSedeTercero { get; set; }
    public string? NomSedeTercero { get; set; }
    public string? NumLicenciaConduccion { get; set; }
    public string? CodCategoriaLicenciaConduccion { get; set; }
    public DateTime? FechaVencimientoLicencia { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? RegimenSimple { get; set; }
}
