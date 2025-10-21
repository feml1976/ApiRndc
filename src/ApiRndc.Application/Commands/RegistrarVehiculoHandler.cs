using ApiRndc.Domain.Entities;
using ApiRndc.Domain.Enums;
using ApiRndc.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Handler para el comando de registrar vehículo
/// </summary>
public class RegistrarVehiculoHandler : IRequestHandler<RegistrarVehiculoCommand, RegistrarVehiculoResult>
{
    private readonly IRndcSoapClient _soapClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegistrarVehiculoHandler> _logger;

    public RegistrarVehiculoHandler(
        IRndcSoapClient soapClient,
        IUnitOfWork unitOfWork,
        ILogger<RegistrarVehiculoHandler> logger)
    {
        _soapClient = soapClient;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<RegistrarVehiculoResult> Handle(RegistrarVehiculoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Iniciando registro de vehículo: {Placa}", request.Vehiculo.NumPlaca);

            // Verificar si el vehículo ya existe
            var existingVehiculo = await _unitOfWork.Vehiculos.FirstOrDefaultAsync(
                v => v.NumPlaca == request.Vehiculo.NumPlaca && v.IsActive,
                cancellationToken);

            if (existingVehiculo != null)
            {
                _logger.LogWarning("El vehículo con placa {Placa} ya está registrado", request.Vehiculo.NumPlaca);
                return new RegistrarVehiculoResult
                {
                    Success = false,
                    ErrorMessage = $"El vehículo con placa {request.Vehiculo.NumPlaca} ya está registrado"
                };
            }

            // Construir XML de variables
            var xmlVariables = BuildXmlVariables(request.Vehiculo);

            // Crear transacción en BD
            var transaction = new RndcTransaction
            {
                TransactionType = TransactionType.RegistroVehiculos,
                Status = TransactionStatus.Pending,
                RequestXml = xmlVariables,
                NitEmpresaTransporte = request.Vehiculo.NumNitEmpresaTransporte,
                ExternalReference = request.Vehiculo.NumPlaca,
                CreatedBy = request.CreatedBy
            };

            await _unitOfWork.RndcTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Enviar al RNDC
            var response = await _soapClient.SendTransactionAsync(
                TransactionType.RegistroVehiculos,
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

            // Si fue exitoso, guardar vehículo
            if (response.IsSuccess)
            {
                var vehiculo = MapToEntity(request.Vehiculo, response.IngresoId, request.CreatedBy);
                await _unitOfWork.Vehiculos.AddAsync(vehiculo, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Registro de vehículo completado. Success: {Success}, IngresoId: {IngresoId}",
                response.IsSuccess, response.IngresoId);

            return new RegistrarVehiculoResult
            {
                Success = response.IsSuccess,
                IngresoId = response.IngresoId,
                ErrorMessage = response.ErrorDetails,
                TransactionId = transaction.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar vehículo: {Placa}", request.Vehiculo.NumPlaca);
            throw;
        }
    }

    private string BuildXmlVariables(Shared.DTOs.VehiculoDto vehiculo)
    {
        var variables = new XElement("variables",
            new XElement("NUMNITEMPRESATRANSPORTE", vehiculo.NumNitEmpresaTransporte),
            new XElement("NUMPLACA", vehiculo.NumPlaca),
            new XElement("CODCONFIGURACIONUNIDADCARGA", vehiculo.CodConfiguracionUnidadCarga)
        );

        // Agregar campos opcionales
        if (!string.IsNullOrWhiteSpace(vehiculo.CodMarcaVehiculoCarga))
            variables.Add(new XElement("CODMARCAVEHICULOCARGA", vehiculo.CodMarcaVehiculoCarga));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodLineaVehiculoCarga))
            variables.Add(new XElement("CODLINEAVEHICULOCARGA", vehiculo.CodLineaVehiculoCarga));

        if (vehiculo.AnoFabricacionVehiculoCarga.HasValue)
            variables.Add(new XElement("ANOFABRICACIONVEHICULOCARGA", vehiculo.AnoFabricacionVehiculoCarga.Value));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodTipoIdPropietario))
            variables.Add(new XElement("CODTIPOIDPROPIETARIO", vehiculo.CodTipoIdPropietario));

        if (!string.IsNullOrWhiteSpace(vehiculo.NumIdPropietario))
            variables.Add(new XElement("NUMIDPROPIETARIO", vehiculo.NumIdPropietario));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodTipoIdTenedor))
            variables.Add(new XElement("CODTIPOIDTENEDOR", vehiculo.CodTipoIdTenedor));

        if (!string.IsNullOrWhiteSpace(vehiculo.NumIdTenedor))
            variables.Add(new XElement("NUMIDTENEDOR", vehiculo.NumIdTenedor));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodTipoCombustible))
            variables.Add(new XElement("CODTIPOCOMBUSTIBLE", vehiculo.CodTipoCombustible));

        if (vehiculo.PesoVehiculoVacio.HasValue)
            variables.Add(new XElement("PESOVEHICULOVACIO", vehiculo.PesoVehiculoVacio.Value));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodColorVehiculoCarga))
            variables.Add(new XElement("CODCOLORVEHICULOCARGA", vehiculo.CodColorVehiculoCarga));

        if (!string.IsNullOrWhiteSpace(vehiculo.CodTipoCarroceria))
            variables.Add(new XElement("CODTIPOCARROCERIA", vehiculo.CodTipoCarroceria));

        if (!string.IsNullOrWhiteSpace(vehiculo.NumNitAseguradoraSoat))
            variables.Add(new XElement("NUMNITASEGURADORASOAT", vehiculo.NumNitAseguradoraSoat));

        if (vehiculo.FechaVencimientoSoat.HasValue)
            variables.Add(new XElement("FECHAVENCIMIENTOSOAT", vehiculo.FechaVencimientoSoat.Value.ToString("dd/MM/yyyy")));

        if (!string.IsNullOrWhiteSpace(vehiculo.NumSeguroSoat))
            variables.Add(new XElement("NUMSEGUROSOAT", vehiculo.NumSeguroSoat));

        if (!string.IsNullOrWhiteSpace(vehiculo.UnidadMedidaCapacidad))
            variables.Add(new XElement("UNIDADMEDIDACAPACIDAD", vehiculo.UnidadMedidaCapacidad));

        return variables.ToString(SaveOptions.DisableFormatting);
    }

    private Vehiculo MapToEntity(Shared.DTOs.VehiculoDto dto, string? ingresoId, string? createdBy)
    {
        return new Vehiculo
        {
            NumNitEmpresaTransporte = dto.NumNitEmpresaTransporte,
            NumPlaca = dto.NumPlaca,
            CodConfiguracionUnidadCarga = dto.CodConfiguracionUnidadCarga,
            CodMarcaVehiculoCarga = dto.CodMarcaVehiculoCarga,
            CodLineaVehiculoCarga = dto.CodLineaVehiculoCarga,
            AnoFabricacionVehiculoCarga = dto.AnoFabricacionVehiculoCarga,
            CodTipoIdPropietario = dto.CodTipoIdPropietario,
            NumIdPropietario = dto.NumIdPropietario,
            CodTipoIdTenedor = dto.CodTipoIdTenedor,
            NumIdTenedor = dto.NumIdTenedor,
            CodTipoCombustible = dto.CodTipoCombustible,
            PesoVehiculoVacio = dto.PesoVehiculoVacio,
            CodColorVehiculoCarga = dto.CodColorVehiculoCarga,
            CodTipoCarroceria = dto.CodTipoCarroceria,
            NumNitAseguradoraSoat = dto.NumNitAseguradoraSoat,
            FechaVencimientoSoat = dto.FechaVencimientoSoat,
            NumSeguroSoat = dto.NumSeguroSoat,
            UnidadMedidaCapacidad = dto.UnidadMedidaCapacidad,
            IngresoId = ingresoId,
            CreatedBy = createdBy
        };
    }
}
