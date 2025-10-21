using ApiRndc.Shared.DTOs;
using MediatR;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Comando para registrar un vehículo en el RNDC (Proceso 12)
/// </summary>
public class RegistrarVehiculoCommand : IRequest<RegistrarVehiculoResult>
{
    public VehiculoDto Vehiculo { get; set; } = null!;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Resultado del registro de vehículo
/// </summary>
public class RegistrarVehiculoResult
{
    public bool Success { get; set; }
    public string? IngresoId { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid TransactionId { get; set; }
}
