using ApiRndc.Shared.DTOs;
using MediatR;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Comando para expedir un manifiesto de carga en el RNDC (Proceso 4)
/// </summary>
public class ExpedirManifiestoCommand : IRequest<ExpedirManifiestoResult>
{
    public ManifiestoDto Manifiesto { get; set; } = null!;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Resultado de la expedición de manifiesto
/// </summary>
public class ExpedirManifiestoResult
{
    public bool Success { get; set; }
    public string? IngresoId { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid TransactionId { get; set; }
    public string? NumManifiestoCarga { get; set; }
}
