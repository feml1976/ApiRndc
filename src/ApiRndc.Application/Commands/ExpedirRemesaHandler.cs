using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Handler para el comando de expedir remesa
/// </summary>
public class ExpedirRemesaHandler : IRequestHandler<ExpedirRemesaCommand, ExpedirRemesaResult>
{
    private readonly IRndcSoapClient _soapClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpedirRemesaHandler> _logger;

    public ExpedirRemesaHandler(
        IRndcSoapClient soapClient,
        IUnitOfWork unitOfWork,
        ILogger<ExpedirRemesaHandler> logger)
    {
        _soapClient = soapClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ExpedirRemesaResult> Handle(ExpedirRemesaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando expedición de remesa: {Consecutivo}", request.Remesa.ConsecutivoRemesa);

            // Verificar si la remesa ya existe
            var existingRemesa = await _unitOfWork.Remesas.FirstOrDefaultAsync(
                r => r.ConsecutivoRemesa == request.Remesa.ConsecutivoRemesa &&
                     r.NumNitEmpresaTransporte == request.Remesa.NumNitEmpresaTransporte &&
                     r.IsActive,
                cancellationToken);

            if (existingRemesa != null)
            {
                _logger.LogWarning("La remesa con consecutivo {Consecutivo} ya existe", request.Remesa.ConsecutivoRemesa);
                return new ExpedirRemesaResult
                {
                    Success = false,
                    ErrorMessage = $"La remesa con consecutivo {request.Remesa.ConsecutivoRemesa} ya está registrada",
                    ConsecutivoRemesa = request.Remesa.ConsecutivoRemesa
                };
            }

            // Construir XML de variables
            var xmlVariables = BuildXmlVariables(request.Remesa);

            // Crear transacción en BD
            var transaction = new RndcTransaction
            {
                TransactionType = TransactionType.ExpedicionRemesa,
                Status = TransactionStatus.Pending,
                RequestXml = xmlVariables,
                NitEmpresaTransporte = request.Remesa.NumNitEmpresaTransporte,
                ExternalReference = request.Remesa.ConsecutivoRemesa,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.RndcTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Enviar al RNDC
            var response = await _soapClient.SendTransactionAsync(
                TransactionType.ExpedicionRemesa,
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

            // Si fue exitoso, guardar remesa
            if (response.IsSuccess)
            {
                var remesa = MapToEntity(request.Remesa, response.IngresoId, request.CreatedBy);
                await _unitOfWork.Remesas.AddAsync(remesa, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Expedición de remesa completada. Success: {Success}, IngresoId: {IngresoId}",
                response.IsSuccess, response.IngresoId);

            return new ExpedirRemesaResult
            {
                Success = response.IsSuccess,
                IngresoId = response.IngresoId,
                ErrorMessage = response.ErrorDetails,
                TransactionId = transaction.Id,
                ConsecutivoRemesa = request.Remesa.ConsecutivoRemesa
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al expedir remesa: {Consecutivo}", request.Remesa.ConsecutivoRemesa);
            throw;
        }
    }

    private string BuildXmlVariables(Shared.DTOs.RemesaDto remesa)
    {
        var variables = new XElement("variables",
            new XElement("NUMNITEMPRESATRANSPORTE", remesa.NumNitEmpresaTransporte),
            new XElement("CONSECUTIVOREMESA", remesa.ConsecutivoRemesa)
        );

        // Campos opcionales
        if (!string.IsNullOrWhiteSpace(remesa.ConsecutivoInformacionCarga))
            variables.Add(new XElement("CONSECUTIVOINFORMACIONCARGA", remesa.ConsecutivoInformacionCarga));

        if (!string.IsNullOrWhiteSpace(remesa.CodOperacionTransporte))
            variables.Add(new XElement("CODOPERACIONTRANSPORTE", remesa.CodOperacionTransporte));

        if (!string.IsNullOrWhiteSpace(remesa.CodNaturalezaCarga))
            variables.Add(new XElement("CODNATURALEZACARGA", remesa.CodNaturalezaCarga));

        if (remesa.CantidadCargada.HasValue)
            variables.Add(new XElement("CANTIDADCARGADA", remesa.CantidadCargada.Value));

        if (!string.IsNullOrWhiteSpace(remesa.UnidadMedidaCapacidad))
            variables.Add(new XElement("UNIDADMEDIDACAPACIDAD", remesa.UnidadMedidaCapacidad));

        if (!string.IsNullOrWhiteSpace(remesa.CodTipoEmpaque))
            variables.Add(new XElement("CODTIPOEMPAQUE", remesa.CodTipoEmpaque));

        if (remesa.PesoContenedorVacio.HasValue)
            variables.Add(new XElement("PESOCONTENEDORVACIO", remesa.PesoContenedorVacio.Value));

        if (!string.IsNullOrWhiteSpace(remesa.MercanciaRemesa))
            variables.Add(new XElement("MERCANCIAREMESA", remesa.MercanciaRemesa));

        if (!string.IsNullOrWhiteSpace(remesa.DescripcionCortaProducto))
            variables.Add(new XElement("DESCRIPCIONCORTAPRODUCTO", remesa.DescripcionCortaProducto));

        // Remitente
        if (!string.IsNullOrWhiteSpace(remesa.CodTipoIdRemitente))
            variables.Add(new XElement("CODTIPOIDREMITENTE", remesa.CodTipoIdRemitente));

        if (!string.IsNullOrWhiteSpace(remesa.NumIdRemitente))
            variables.Add(new XElement("NUMIDREMITENTE", remesa.NumIdRemitente));

        if (!string.IsNullOrWhiteSpace(remesa.CodSedeRemitente))
            variables.Add(new XElement("CODSEDEREMITENTE", remesa.CodSedeRemitente));

        // Destinatario
        if (!string.IsNullOrWhiteSpace(remesa.CodTipoIdDestinatario))
            variables.Add(new XElement("CODTIPOIDDESTINATARIO", remesa.CodTipoIdDestinatario));

        if (!string.IsNullOrWhiteSpace(remesa.NumIdDestinatario))
            variables.Add(new XElement("NUMIDDESTINATARIO", remesa.NumIdDestinatario));

        if (!string.IsNullOrWhiteSpace(remesa.CodSedeDestinatario))
            variables.Add(new XElement("CODSEDEDESTINATARIO", remesa.CodSedeDestinatario));

        // Póliza
        if (!string.IsNullOrWhiteSpace(remesa.DuenoPoliza))
            variables.Add(new XElement("DUENOPOLIZA", remesa.DuenoPoliza));

        if (!string.IsNullOrWhiteSpace(remesa.NumPolizaTransporte))
            variables.Add(new XElement("NUMPOLIZATRANSPORTE", remesa.NumPolizaTransporte));

        if (!string.IsNullOrWhiteSpace(remesa.CompaniaSeguro))
            variables.Add(new XElement("COMPANIASEGURO", remesa.CompaniaSeguro));

        if (remesa.FechaVencimientoPolizaCarga.HasValue)
            variables.Add(new XElement("FECHAVENCIMIENTOPOLIZACARGA", remesa.FechaVencimientoPolizaCarga.Value.ToString("dd/MM/yyyy")));

        // Tiempos de cargue/descargue
        if (remesa.HorasPactoCarga.HasValue)
            variables.Add(new XElement("HORASPACTOCARGA", remesa.HorasPactoCarga.Value));

        if (remesa.MinutosPactoCarga.HasValue)
            variables.Add(new XElement("MINUTOSPACTOCARGA", remesa.MinutosPactoCarga.Value));

        if (remesa.HorasPactoDescargue.HasValue)
            variables.Add(new XElement("HORASPACTODESCARGUE", remesa.HorasPactoDescargue.Value));

        if (remesa.MinutosPactoDescargue.HasValue)
            variables.Add(new XElement("MINUTOSPACTODESCARGUE", remesa.MinutosPactoDescargue.Value));

        // Propietario
        if (!string.IsNullOrWhiteSpace(remesa.CodTipoIdPropietario))
            variables.Add(new XElement("CODTIPOIDPROPIETARIO", remesa.CodTipoIdPropietario));

        if (!string.IsNullOrWhiteSpace(remesa.NumIdPropietario))
            variables.Add(new XElement("NUMIDPROPIETARIO", remesa.NumIdPropietario));

        if (!string.IsNullOrWhiteSpace(remesa.CodSedePropietario))
            variables.Add(new XElement("CODSEDEPROPIETARIO", remesa.CodSedePropietario));

        if (!string.IsNullOrWhiteSpace(remesa.OrdenServicioGenerador))
            variables.Add(new XElement("ORDENSERVICIOGENERADOR", remesa.OrdenServicioGenerador));

        // Fechas de cita
        if (remesa.FechaCitaPactadaCargue.HasValue)
            variables.Add(new XElement("FECHACITAPACTADACARGUE", remesa.FechaCitaPactadaCargue.Value.ToString("dd/MM/yyyy")));

        if (!string.IsNullOrWhiteSpace(remesa.HoraCitaPactadaCargue))
            variables.Add(new XElement("HORACITAPACTADACARGUE", remesa.HoraCitaPactadaCargue));

        if (remesa.FechaCitaPactadaDescargue.HasValue)
            variables.Add(new XElement("FECHACITAPACTADADESCARGUE", remesa.FechaCitaPactadaDescargue.Value.ToString("dd/MM/yyyy")));

        if (!string.IsNullOrWhiteSpace(remesa.HoraCitaPactadaDescargueRemesa))
            variables.Add(new XElement("HORACITAPACTADADESCARGUEREMESA", remesa.HoraCitaPactadaDescargueRemesa));

        // Otros campos
        if (!string.IsNullOrWhiteSpace(remesa.PermisoCargaExtra))
            variables.Add(new XElement("PERMISOCARGAEXTRA", remesa.PermisoCargaExtra));

        if (!string.IsNullOrWhiteSpace(remesa.NumIdGps))
            variables.Add(new XElement("NUMIDGPS", remesa.NumIdGps));

        if (!string.IsNullOrWhiteSpace(remesa.CodigoUn))
            variables.Add(new XElement("CODIGOUN", remesa.CodigoUn));

        if (!string.IsNullOrWhiteSpace(remesa.SubpartidaCode))
            variables.Add(new XElement("SUBPARTIDA_CODE", remesa.SubpartidaCode));

        if (!string.IsNullOrWhiteSpace(remesa.CodigoArancelCode))
            variables.Add(new XElement("CODIGOARANCEL_CODE", remesa.CodigoArancelCode));

        if (!string.IsNullOrWhiteSpace(remesa.GrupoEmbalajeEnvase))
            variables.Add(new XElement("GRUPOEMBALAJEENVASE", remesa.GrupoEmbalajeEnvase));

        if (!string.IsNullOrWhiteSpace(remesa.EstadoMercancia))
            variables.Add(new XElement("ESTADOMERCANCIA", remesa.EstadoMercancia));

        if (!string.IsNullOrWhiteSpace(remesa.UnidadMedidaProducto))
            variables.Add(new XElement("UNIDADMEDIDAPRODUCTO", remesa.UnidadMedidaProducto));

        if (remesa.CantidadProducto.HasValue)
            variables.Add(new XElement("CANTIDADPRODUCTO", remesa.CantidadProducto.Value));

        return variables.ToString(SaveOptions.DisableFormatting);
    }

    private Remesa MapToEntity(Shared.DTOs.RemesaDto dto, string? ingresoId, string? createdBy)
    {
        return new Remesa
        {
            NumNitEmpresaTransporte = dto.NumNitEmpresaTransporte,
            ConsecutivoRemesa = dto.ConsecutivoRemesa,
            ConsecutivoInformacionCarga = dto.ConsecutivoInformacionCarga,
            CodOperacionTransporte = dto.CodOperacionTransporte,
            CodNaturalezaCarga = dto.CodNaturalezaCarga,
            CantidadCargada = dto.CantidadCargada,
            UnidadMedidaCapacidad = dto.UnidadMedidaCapacidad,
            CodTipoEmpaque = dto.CodTipoEmpaque,
            PesoContenedorVacio = dto.PesoContenedorVacio,
            MercanciaRemesa = dto.MercanciaRemesa,
            DescripcionCortaProducto = dto.DescripcionCortaProducto,
            CodTipoIdRemitente = dto.CodTipoIdRemitente,
            NumIdRemitente = dto.NumIdRemitente,
            CodSedeRemitente = dto.CodSedeRemitente,
            CodTipoIdDestinatario = dto.CodTipoIdDestinatario,
            NumIdDestinatario = dto.NumIdDestinatario,
            CodSedeDestinatario = dto.CodSedeDestinatario,
            DuenoPoliza = dto.DuenoPoliza,
            NumPolizaTransporte = dto.NumPolizaTransporte,
            CompaniaSeguro = dto.CompaniaSeguro,
            FechaVencimientoPolizaCarga = dto.FechaVencimientoPolizaCarga,
            HorasPactoCarga = dto.HorasPactoCarga,
            MinutosPactoCarga = dto.MinutosPactoCarga,
            HorasPactoDescargue = dto.HorasPactoDescargue,
            MinutosPactoDescargue = dto.MinutosPactoDescargue,
            CodTipoIdPropietario = dto.CodTipoIdPropietario,
            NumIdPropietario = dto.NumIdPropietario,
            CodSedePropietario = dto.CodSedePropietario,
            OrdenServicioGenerador = dto.OrdenServicioGenerador,
            FechaCitaPactadaCargue = dto.FechaCitaPactadaCargue,
            HoraCitaPactadaCargue = !string.IsNullOrWhiteSpace(dto.HoraCitaPactadaCargue) ? TimeSpan.Parse(dto.HoraCitaPactadaCargue) : null,
            FechaCitaPactadaDescargue = dto.FechaCitaPactadaDescargue,
            HoraCitaPactadaDescargueRemesa = !string.IsNullOrWhiteSpace(dto.HoraCitaPactadaDescargueRemesa) ? TimeSpan.Parse(dto.HoraCitaPactadaDescargueRemesa) : null,
            PermisoCargaExtra = dto.PermisoCargaExtra,
            NumIdGps = dto.NumIdGps,
            CodigoUn = dto.CodigoUn,
            SubpartidaCode = dto.SubpartidaCode,
            CodigoArancelCode = dto.CodigoArancelCode,
            GrupoEmbalajeEnvase = dto.GrupoEmbalajeEnvase,
            EstadoMercancia = dto.EstadoMercancia,
            UnidadMedidaProducto = dto.UnidadMedidaProducto,
            CantidadProducto = dto.CantidadProducto,
            IngresoId = ingresoId,
            CreatedBy = createdBy
        };
    }
}
