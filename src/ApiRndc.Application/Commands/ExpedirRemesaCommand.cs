using ApiRndc.Shared.DTOs;
using MediatR;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Comando para expedir una remesa terrestre de carga en el RNDC (Proceso 3)
/// </summary>
public class ExpedirRemesaCommand : IRequest<ExpedirRemesaResult>
{
    public RemesaDto Remesa { get; set; } = null!;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Resultado de la expedición de remesa
/// </summary>
public class ExpedirRemesaResult
{
    public bool Success { get; set; }
    public string? IngresoId { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid TransactionId { get; set; }
    public string? ConsecutivoRemesa { get; set; }
}
