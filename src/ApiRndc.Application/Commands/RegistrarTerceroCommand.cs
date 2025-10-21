using ApiRndc.Shared.DTOs;
using MediatR;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Comando para registrar un tercero en el RNDC (Proceso 11)
/// </summary>
public class RegistrarTerceroCommand : IRequest<RegistrarTerceroResult>
{
    public TerceroDto Tercero { get; set; } = null!;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Resultado del registro de tercero
/// </summary>
public class RegistrarTerceroResult
{
    public bool Success { get; set; }
    public string? IngresoId { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid TransactionId { get; set; }
}
