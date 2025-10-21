using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Handler para el comando de registrar cumplido (remesa o manifiesto)
/// </summary>
public class RegistrarCumplidoHandler : IRequestHandler<RegistrarCumplidoCommand, RegistrarCumplidoResult>
{
    private readonly IRndcSoapClient _soapClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegistrarCumplidoHandler> _logger;

    public RegistrarCumplidoHandler(
        IRndcSoapClient soapClient,
        IUnitOfWork unitOfWork,
        ILogger<RegistrarCumplidoHandler> logger)
    {
        _soapClient = soapClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RegistrarCumplidoResult> Handle(RegistrarCumplidoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tipoCumplido = request.TipoCumplido == TransactionType.CumplidoRemesa ? "Remesa" : "Manifiesto";
            _logger.LogInformation("Iniciando cumplido de {Tipo}: {Consecutivo}", tipoCumplido, request.Cumplido.Consecutivo);

            // Validar que exista la remesa o manifiesto
            if (request.TipoCumplido == TransactionType.CumplidoRemesa)
            {
                var remesa = await _unitOfWork.Remesas.FirstOrDefaultAsync(
                    r => r.ConsecutivoRemesa == request.Cumplido.Consecutivo && r.IsActive,
                    cancellationToken);

                if (remesa == null)
                {
                    _logger.LogWarning("La remesa {Consecutivo} no existe", request.Cumplido.Consecutivo);
                    return new RegistrarCumplidoResult
                    {
                        Success = false,
                        ErrorMessage = $"La remesa {request.Cumplido.Consecutivo} no existe o no ha sido expedida",
                        Consecutivo = request.Cumplido.Consecutivo,
                        TipoCumplido = request.TipoCumplido
                    };
                }

                if (string.IsNullOrWhiteSpace(remesa.IngresoId))
                {
                    return new RegistrarCumplidoResult
                    {
                        Success = false,
                        ErrorMessage = $"La remesa {request.Cumplido.Consecutivo} no tiene IngresoId, no se puede dar cumplido",
                        Consecutivo = request.Cumplido.Consecutivo,
                        TipoCumplido = request.TipoCumplido
                    };
                }
            }
            else if (request.TipoCumplido == TransactionType.CumplidoManifiesto)
            {
                var manifiesto = await _unitOfWork.Manifiestos.FirstOrDefaultAsync(
                    m => m.NumManifiestoCarga == request.Cumplido.Consecutivo && m.IsActive,
                    cancellationToken);

                if (manifiesto == null)
                {
                    _logger.LogWarning("El manifiesto {Consecutivo} no existe", request.Cumplido.Consecutivo);
                    return new RegistrarCumplidoResult
                    {
                        Success = false,
                        ErrorMessage = $"El manifiesto {request.Cumplido.Consecutivo} no existe o no ha sido expedido",
                        Consecutivo = request.Cumplido.Consecutivo,
                        TipoCumplido = request.TipoCumplido
                    };
                }

                if (string.IsNullOrWhiteSpace(manifiesto.IngresoId))
                {
                    return new RegistrarCumplidoResult
                    {
                        Success = false,
                        ErrorMessage = $"El manifiesto {request.Cumplido.Consecutivo} no tiene IngresoId, no se puede dar cumplido",
                        Consecutivo = request.Cumplido.Consecutivo,
                        TipoCumplido = request.TipoCumplido
                    };
                }
            }

            // Construir XML de variables
            var xmlVariables = BuildXmlVariables(request.Cumplido, request.TipoCumplido);

            // Crear transacción en BD
            var transaction = new RndcTransaction
            {
                TransactionType = request.TipoCumplido,
                Status = TransactionStatus.Pending,
                RequestXml = xmlVariables,
                NitEmpresaTransporte = request.Cumplido.NumNitEmpresaTransporte,
                ExternalReference = request.Cumplido.Consecutivo,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.RndcTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Enviar al RNDC
            var response = await _soapClient.SendTransactionAsync(
                request.TipoCumplido,
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Cumplido de {Tipo} completado. Success: {Success}, IngresoId: {IngresoId}",
                tipoCumplido, response.IsSuccess, response.IngresoId);

            return new RegistrarCumplidoResult
            {
                Success = response.IsSuccess,
                IngresoId = response.IngresoId,
                ErrorMessage = response.ErrorDetails,
                TransactionId = transaction.Id,
                Consecutivo = request.Cumplido.Consecutivo,
                TipoCumplido = request.TipoCumplido
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar cumplido: {Consecutivo}", request.Cumplido.Consecutivo);
            throw;
        }
    }

    private string BuildXmlVariables(Shared.DTOs.CumplidoDto cumplido, TransactionType tipoCumplido)
    {
        var variables = new XElement("variables",
            new XElement("NUMNITEMPRESATRANSPORTE", cumplido.NumNitEmpresaTransporte)
        );

        // Campo consecutivo depende del tipo
        if (tipoCumplido == TransactionType.CumplidoRemesa)
        {
            variables.Add(new XElement("CONSECUTIVOREMESA", cumplido.Consecutivo));
        }
        else if (tipoCumplido == TransactionType.CumplidoManifiesto)
        {
            variables.Add(new XElement("NUMMANIFIESTOCARGA", cumplido.Consecutivo));
        }

        // Fecha y hora del cumplido
        variables.Add(new XElement("FECHACUMPLIDO", cumplido.FechaCumplido.ToString("dd/MM/yyyy")));
        variables.Add(new XElement("HORACUMPLIDO", cumplido.FechaCumplido.ToString("HH:mm")));

        // Coordenadas GPS
        if (cumplido.Latitud.HasValue)
            variables.Add(new XElement("LATITUD", cumplido.Latitud.Value.ToString("0.000000")));

        if (cumplido.Longitud.HasValue)
            variables.Add(new XElement("LONGITUD", cumplido.Longitud.Value.ToString("0.000000")));

        // Observaciones
        if (!string.IsNullOrWhiteSpace(cumplido.Observaciones))
            variables.Add(new XElement("OBSERVACIONES", cumplido.Observaciones));

        return variables.ToString(SaveOptions.DisableFormatting);
    }
}
