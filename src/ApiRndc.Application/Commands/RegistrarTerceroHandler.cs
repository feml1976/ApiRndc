using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Linq;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Handler para el comando de registrar tercero
/// </summary>
public class RegistrarTerceroHandler : IRequestHandler<RegistrarTerceroCommand, RegistrarTerceroResult>
{
    private readonly IRndcSoapClient _soapClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegistrarTerceroHandler> _logger;

    public RegistrarTerceroHandler(
        IRndcSoapClient soapClient,
        IUnitOfWork unitOfWork,
        ILogger<RegistrarTerceroHandler> logger)
    {
        _soapClient = soapClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RegistrarTerceroResult> Handle(RegistrarTerceroCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando registro de tercero: {NumId}", request.Tercero.NumIdTercero);

            // Construir XML de variables
            var xmlVariables = BuildXmlVariables(request.Tercero);

            // Crear transacción en BD
            var transaction = new RndcTransaction
            {
                TransactionType = TransactionType.RegistroTerceros,
                Status = TransactionStatus.Pending,
                RequestXml = xmlVariables,
                NitEmpresaTransporte = request.Tercero.NumNitEmpresaTransporte,
                ExternalReference = request.Tercero.NumIdTercero,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.RndcTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Enviar al RNDC
            var response = await _soapClient.SendTransactionAsync(
                TransactionType.RegistroTerceros,
                xmlVariables,
                cancellationToken);

            // Actualizar transacción con respuesta
            transaction.ResponseXml = response.ResponseXml;
            transaction.Status = response.IsSuccess ? TransactionStatus.Success : TransactionStatus.Failed;
            transaction.IngresoId = response.IngresoId;
            transaction.ErrorMessage = response.ErrorDetails;
            transaction.ErrorCode = response.ErrorCode;
            transaction.SuccessAt = response.IsSuccess ? DateTime.UtcNow : null;

            await _unitOfWork.RndcTransactions.UpdateAsync(transaction, cancellationToken);

            // Si fue exitoso, guardar o actualizar tercero
            if (response.IsSuccess)
            {
                var tercero = await MapToEntity(request.Tercero, response.IngresoId, request.CreatedBy);

                var existingTercero = await _unitOfWork.Terceros.FirstOrDefaultAsync(
                    t => t.NumIdTercero == request.Tercero.NumIdTercero &&
                         t.NumNitEmpresaTransporte == request.Tercero.NumNitEmpresaTransporte,
                    cancellationToken);

                if (existingTercero == null)
                {
                    await _unitOfWork.Terceros.AddAsync(tercero, cancellationToken);
                }
                else
                {
                    existingTercero.IngresoId = response.IngresoId;
                    existingTercero.UpdatedAt = DateTime.UtcNow;
                    existingTercero.UpdatedBy = request.CreatedBy;
                    await _unitOfWork.Terceros.UpdateAsync(existingTercero, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Registro de tercero completado. Success: {Success}, IngresoId: {IngresoId}",
                response.IsSuccess, response.IngresoId);

            return new RegistrarTerceroResult
            {
                Success = response.IsSuccess,
                IngresoId = response.IngresoId,
                ErrorMessage = response.ErrorDetails,
                TransactionId = transaction.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar tercero: {NumId}", request.Tercero.NumIdTercero);
            throw;
        }
    }

    private string BuildXmlVariables(Shared.DTOs.TerceroDto tercero)
    {
        var variables = new XElement("variables",
            new XElement("NUMNITEMPRESATRANSPORTE", tercero.NumNitEmpresaTransporte),
            new XElement("CODTIPOIDTERCERO", tercero.CodTipoIdTercero),
            new XElement("NUMIDTERCERO", tercero.NumIdTercero),
            new XElement("NOMIDTERCERO", tercero.NomIdTercero)
        );

        if (!string.IsNullOrWhiteSpace(tercero.PrimerApellidoIdTercero))
            variables.Add(new XElement("PRIMERAPELLIDOIDTERCERO", tercero.PrimerApellidoIdTercero));

        if (!string.IsNullOrWhiteSpace(tercero.SegundoApellidoIdTercero))
            variables.Add(new XElement("SEGUNDOAPELLIDOIDTERCERO", tercero.SegundoApellidoIdTercero));

        if (!string.IsNullOrWhiteSpace(tercero.NumTelefonoContacto))
            variables.Add(new XElement("NUMTELEFONOCONTACTO", tercero.NumTelefonoContacto));

        if (!string.IsNullOrWhiteSpace(tercero.NomenclaturaDireccion))
            variables.Add(new XElement("NOMENCLATURADIRECCION", tercero.NomenclaturaDireccion));

        if (!string.IsNullOrWhiteSpace(tercero.CodMunicipioRndc))
            variables.Add(new XElement("CODMUNICIPIORNDC", tercero.CodMunicipioRndc));

        if (!string.IsNullOrWhiteSpace(tercero.CodSedeTercero))
            variables.Add(new XElement("CODSEDETERCERO", tercero.CodSedeTercero));

        if (!string.IsNullOrWhiteSpace(tercero.NomSedeTercero))
            variables.Add(new XElement("NOMSEDETERCERO", tercero.NomSedeTercero));

        if (!string.IsNullOrWhiteSpace(tercero.NumLicenciaConduccion))
            variables.Add(new XElement("NUMLICENCIACONDUCCION", tercero.NumLicenciaConduccion));

        if (!string.IsNullOrWhiteSpace(tercero.CodCategoriaLicenciaConduccion))
            variables.Add(new XElement("CODCATEGORIALICENCIACONDUCCION", tercero.CodCategoriaLicenciaConduccion));

        if (tercero.FechaVencimientoLicencia.HasValue)
            variables.Add(new XElement("FECHAVENCIMIENTOLICENCIA", tercero.FechaVencimientoLicencia.Value.ToString("yyyy-MM-dd")));

        if (tercero.Latitud.HasValue)
            variables.Add(new XElement("LATITUD", tercero.Latitud.Value.ToString("0.000000")));

        if (tercero.Longitud.HasValue)
            variables.Add(new XElement("LONGITUD", tercero.Longitud.Value.ToString("0.000000")));

        if (!string.IsNullOrWhiteSpace(tercero.RegimenSimple))
            variables.Add(new XElement("REGIMENSIMPLE", tercero.RegimenSimple));

        return variables.ToString(SaveOptions.DisableFormatting);
    }

    private Task<Tercero> MapToEntity(Shared.DTOs.TerceroDto dto, string? ingresoId, string? createdBy)
    {
        var tercero = new Tercero
        {
            NumNitEmpresaTransporte = dto.NumNitEmpresaTransporte,
            CodTipoIdTercero = dto.CodTipoIdTercero,
            NumIdTercero = dto.NumIdTercero,
            NomIdTercero = dto.NomIdTercero,
            PrimerApellidoIdTercero = dto.PrimerApellidoIdTercero,
            SegundoApellidoIdTercero = dto.SegundoApellidoIdTercero,
            NumTelefonoContacto = dto.NumTelefonoContacto,
            NomenclaturaDireccion = dto.NomenclaturaDireccion,
            CodMunicipioRndc = dto.CodMunicipioRndc,
            CodSedeTercero = dto.CodSedeTercero,
            NomSedeTercero = dto.NomSedeTercero,
            NumLicenciaConduccion = dto.NumLicenciaConduccion,
            CodCategoriaLicenciaConduccion = dto.CodCategoriaLicenciaConduccion,
            FechaVencimientoLicencia = dto.FechaVencimientoLicencia,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            RegimenSimple = dto.RegimenSimple,
            IngresoId = ingresoId,
            CreatedBy = createdBy
        };

        return Task.FromResult(tercero);
    }
}
