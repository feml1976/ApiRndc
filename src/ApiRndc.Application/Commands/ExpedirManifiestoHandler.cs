using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Handler para el comando de expedir manifiesto
/// </summary>
public class ExpedirManifiestoHandler : IRequestHandler<ExpedirManifiestoCommand, ExpedirManifiestoResult>
{
    private readonly IRndcSoapClient _soapClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpedirManifiestoHandler> _logger;

    public ExpedirManifiestoHandler(
        IRndcSoapClient soapClient,
        IUnitOfWork unitOfWork,
        ILogger<ExpedirManifiestoHandler> logger)
    {
        _soapClient = soapClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ExpedirManifiestoResult> Handle(ExpedirManifiestoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando expedición de manifiesto: {NumManifiesto}", request.Manifiesto.NumManifiestoCarga);

            // Verificar si el manifiesto ya existe
            var existingManifiesto = await _unitOfWork.Manifiestos.FirstOrDefaultAsync(
                m => m.NumManifiestoCarga == request.Manifiesto.NumManifiestoCarga &&
                     m.NumNitEmpresaTransporte == request.Manifiesto.NumNitEmpresaTransporte &&
                     m.IsActive,
                cancellationToken);

            if (existingManifiesto != null)
            {
                _logger.LogWarning("El manifiesto {NumManifiesto} ya existe", request.Manifiesto.NumManifiestoCarga);
                return new ExpedirManifiestoResult
                {
                    Success = false,
                    ErrorMessage = $"El manifiesto {request.Manifiesto.NumManifiestoCarga} ya está registrado",
                    NumManifiestoCarga = request.Manifiesto.NumManifiestoCarga
                };
            }

            // Validar que existan las remesas asociadas
            if (request.Manifiesto.ConsecutivosRemesas != null && request.Manifiesto.ConsecutivosRemesas.Any())
            {
                foreach (var consecutivo in request.Manifiesto.ConsecutivosRemesas)
                {
                    var remesa = await _unitOfWork.Remesas.FirstOrDefaultAsync(
                        r => r.ConsecutivoRemesa == consecutivo && r.IsActive,
                        cancellationToken);

                    if (remesa == null)
                    {
                        _logger.LogWarning("La remesa {Consecutivo} no existe", consecutivo);
                        return new ExpedirManifiestoResult
                        {
                            Success = false,
                            ErrorMessage = $"La remesa {consecutivo} no existe o no ha sido expedida",
                            NumManifiestoCarga = request.Manifiesto.NumManifiestoCarga
                        };
                    }
                }
            }

            // Construir XML de variables
            var xmlVariables = BuildXmlVariables(request.Manifiesto);

            // Crear transacción en BD
            var transaction = new RndcTransaction
            {
                TransactionType = TransactionType.ExpedicionManifiesto,
                Status = TransactionStatus.Pending,
                RequestXml = xmlVariables,
                NitEmpresaTransporte = request.Manifiesto.NumNitEmpresaTransporte,
                ExternalReference = request.Manifiesto.NumManifiestoCarga,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.RndcTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Enviar al RNDC
            var response = await _soapClient.SendTransactionAsync(
                TransactionType.ExpedicionManifiesto,
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

            // Si fue exitoso, guardar manifiesto y relaciones
            if (response.IsSuccess)
            {
                var manifiesto = MapToEntity(request.Manifiesto, response.IngresoId, request.CreatedBy);
                await _unitOfWork.Manifiestos.AddAsync(manifiesto, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Crear relaciones con remesas
                if (request.Manifiesto.ConsecutivosRemesas != null)
                {
                    foreach (var consecutivo in request.Manifiesto.ConsecutivosRemesas)
                    {
                        var remesa = await _unitOfWork.Remesas.FirstOrDefaultAsync(
                            r => r.ConsecutivoRemesa == consecutivo,
                            cancellationToken);

                        var manifiestoRemesa = new ManifiestoRemesa
                        {
                            ManifiestoId = manifiesto.Id,
                            ConsecutivoRemesa = consecutivo,
                            RemesaId = remesa?.Id,
                            CreatedBy = request.CreatedBy
                        };

                        await _unitOfWork.ManifiestoRemesas.AddAsync(manifiestoRemesa, cancellationToken);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Expedición de manifiesto completada. Success: {Success}, IngresoId: {IngresoId}",
                response.IsSuccess, response.IngresoId);

            return new ExpedirManifiestoResult
            {
                Success = response.IsSuccess,
                IngresoId = response.IngresoId,
                ErrorMessage = response.ErrorDetails,
                TransactionId = transaction.Id,
                NumManifiestoCarga = request.Manifiesto.NumManifiestoCarga
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al expedir manifiesto: {NumManifiesto}", request.Manifiesto.NumManifiestoCarga);
            throw;
        }
    }

    private string BuildXmlVariables(Shared.DTOs.ManifiestoDto manifiesto)
    {
        var variablesXml = $@"<NUMNITEMPRESATRANSPORTE>{manifiesto.NumNitEmpresaTransporte}</NUMNITEMPRESATRANSPORTE>
<NUMMANIFIESTOCARGA>{manifiesto.NumManifiestoCarga}</NUMMANIFIESTOCARGA>";

        if (!string.IsNullOrWhiteSpace(manifiesto.ConsecutivoInformacionViaje))
            variablesXml += $"<CONSECUTIVOINFORMACIONVIAJE>{manifiesto.ConsecutivoInformacionViaje}</CONSECUTIVOINFORMACIONVIAJE>";

        if (!string.IsNullOrWhiteSpace(manifiesto.ManNroManifiestoTransbordo))
            variablesXml += $"<MANNROMANIFIESTOTRANSBORDO>{manifiesto.ManNroManifiestoTransbordo}</MANNROMANIFIESTOTRANSBORDO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodOperacionTransporte))
            variablesXml += $"<CODOPERACIONTRANSPORTE>{manifiesto.CodOperacionTransporte}</CODOPERACIONTRANSPORTE>";

        if (manifiesto.FechaExpedicionManifiesto.HasValue)
            variablesXml += $"<FECHAEXPEDICIONMANIFIESTO>{manifiesto.FechaExpedicionManifiesto.Value:dd/MM/yyyy}</FECHAEXPEDICIONMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodMunicipioOrigenManifiesto))
            variablesXml += $"<CODMUNICIPIOORIGENMANIFIESTO>{manifiesto.CodMunicipioOrigenManifiesto}</CODMUNICIPIOORIGENMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodMunicipioDestinoManifiesto))
            variablesXml += $"<CODMUNICIPIODESTINOMANIFIESTO>{manifiesto.CodMunicipioDestinoManifiesto}</CODMUNICIPIODESTINOMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodIdTitularManifiesto))
            variablesXml += $"<CODIDTITULARMANIFIESTO>{manifiesto.CodIdTitularManifiesto}</CODIDTITULARMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NumIdTitularManifiesto))
            variablesXml += $"<NUMIDTITULARMANIFIESTO>{manifiesto.NumIdTitularManifiesto}</NUMIDTITULARMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NumPlaca))
            variablesXml += $"<NUMPLACA>{manifiesto.NumPlaca}</NUMPLACA>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NumPlacaRemolque))
            variablesXml += $"<NUMPLACAREMOLQUE>{manifiesto.NumPlacaRemolque}</NUMPLACAREMOLQUE>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodIdConductor))
            variablesXml += $"<CODIDCONDUCTOR>{manifiesto.CodIdConductor}</CODIDCONDUCTOR>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NumIdConductor))
            variablesXml += $"<NUMIDCONDUCTOR>{manifiesto.NumIdConductor}</NUMIDCONDUCTOR>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodIdConductor2))
            variablesXml += $"<CODIDCONDUCTOR2>{manifiesto.CodIdConductor2}</CODIDCONDUCTOR2>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NumIdConductor2))
            variablesXml += $"<NUMIDCONDUCTOR2>{manifiesto.NumIdConductor2}</NUMIDCONDUCTOR2>";

        if (manifiesto.ValorFletePactadoViaje.HasValue)
            variablesXml += $"<VALORFLETEPACTADOVIAJE>{manifiesto.ValorFletePactadoViaje.Value}</VALORFLETEPACTADOVIAJE>";

        if (manifiesto.RetencionFuenteManifiesto.HasValue)
            variablesXml += $"<RETENCIONFUENTEMANIFIESTO>{manifiesto.RetencionFuenteManifiesto.Value}</RETENCIONFUENTEMANIFIESTO>";

        if (manifiesto.RetencionIcaManifiestoCarga.HasValue)
            variablesXml += $"<RETENCIONICAMANIFIESTOCARGA>{manifiesto.RetencionIcaManifiestoCarga.Value}</RETENCIONICAMANIFIESTOCARGA>";

        if (manifiesto.ValorAnticipoManifiesto.HasValue)
            variablesXml += $"<VALORANTICIPOMANIFIESTO>{manifiesto.ValorAnticipoManifiesto.Value}</VALORANTICIPOMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodMunicipioPagoSaldo))
            variablesXml += $"<CODMUNICIPIOPAGOSALDO>{manifiesto.CodMunicipioPagoSaldo}</CODMUNICIPIOPAGOSALDO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodResponsablePagoCargue))
            variablesXml += $"<CODRESPONSABLEPAGOCARGUE>{manifiesto.CodResponsablePagoCargue}</CODRESPONSABLEPAGOCARGUE>";

        if (!string.IsNullOrWhiteSpace(manifiesto.CodResponsablePagoDescargue))
            variablesXml += $"<CODRESPONSABLEPAGODESCARGUE>{manifiesto.CodResponsablePagoDescargue}</CODRESPONSABLEPAGODESCARGUE>";

        if (manifiesto.FechaPagoSaldoManifiesto.HasValue)
            variablesXml += $"<FECHAPAGOSALDOMANIFIESTO>{manifiesto.FechaPagoSaldoManifiesto.Value:dd/MM/yyyy}</FECHAPAGOSALDOMANIFIESTO>";

        if (!string.IsNullOrWhiteSpace(manifiesto.NitMonitoreoFlota))
            variablesXml += $"<NITMONITOREOFLOTA>{manifiesto.NitMonitoreoFlota}</NITMONITOREOFLOTA>";

        if (!string.IsNullOrWhiteSpace(manifiesto.AceptacionElectronica))
            variablesXml += $"<ACEPTACIONELECTRONICA>{manifiesto.AceptacionElectronica}</ACEPTACIONELECTRONICA>";

        if (!string.IsNullOrWhiteSpace(manifiesto.Observaciones))
            variablesXml += $"<OBSERVACIONES>{manifiesto.Observaciones}</OBSERVACIONES>";

        // Agregar remesas asociadas
        if (manifiesto.ConsecutivosRemesas != null && manifiesto.ConsecutivosRemesas.Any())
        {
            variablesXml += "<REMESASMAN procesoid=\"43\">";
            foreach (var consecutivo in manifiesto.ConsecutivosRemesas)
            {
                variablesXml += $"<REMESA><CONSECUTIVOREMESA>{consecutivo}</CONSECUTIVOREMESA></REMESA>";
            }
            variablesXml += "</REMESASMAN>";

            // También agregar el último consecutivo
            variablesXml += $"<CONSECUTIVOREMESA>{manifiesto.ConsecutivosRemesas.Last()}</CONSECUTIVOREMESA>";
        }

        return variablesXml;
    }

    private Manifiesto MapToEntity(Shared.DTOs.ManifiestoDto dto, string? ingresoId, string? createdBy)
    {
        return new Manifiesto
        {
            NumNitEmpresaTransporte = dto.NumNitEmpresaTransporte,
            NumManifiestoCarga = dto.NumManifiestoCarga,
            ConsecutivoInformacionViaje = dto.ConsecutivoInformacionViaje,
            ManNroManifiestoTransbordo = dto.ManNroManifiestoTransbordo,
            CodOperacionTransporte = dto.CodOperacionTransporte,
            FechaExpedicionManifiesto = dto.FechaExpedicionManifiesto,
            CodMunicipioOrigenManifiesto = dto.CodMunicipioOrigenManifiesto,
            CodMunicipioDestinoManifiesto = dto.CodMunicipioDestinoManifiesto,
            CodIdTitularManifiesto = dto.CodIdTitularManifiesto,
            NumIdTitularManifiesto = dto.NumIdTitularManifiesto,
            NumPlaca = dto.NumPlaca,
            NumPlacaRemolque = dto.NumPlacaRemolque,
            CodIdConductor = dto.CodIdConductor,
            NumIdConductor = dto.NumIdConductor,
            CodIdConductor2 = dto.CodIdConductor2,
            NumIdConductor2 = dto.NumIdConductor2,
            ValorFletePactadoViaje = dto.ValorFletePactadoViaje,
            RetencionFuenteManifiesto = dto.RetencionFuenteManifiesto,
            RetencionIcaManifiestoCarga = dto.RetencionIcaManifiestoCarga,
            ValorAnticipoManifiesto = dto.ValorAnticipoManifiesto,
            CodMunicipioPagoSaldo = dto.CodMunicipioPagoSaldo,
            CodResponsablePagoCargue = dto.CodResponsablePagoCargue,
            CodResponsablePagoDescargue = dto.CodResponsablePagoDescargue,
            FechaPagoSaldoManifiesto = dto.FechaPagoSaldoManifiesto,
            NitMonitoreoFlota = dto.NitMonitoreoFlota,
            AceptacionElectronica = dto.AceptacionElectronica,
            Observaciones = dto.Observaciones,
            RemesasAsociadas = dto.ConsecutivosRemesas != null ? string.Join(",", dto.ConsecutivosRemesas) : null,
            IngresoId = ingresoId,
            CreatedBy = createdBy
        };
    }
}
