using ApiRndc.Domain.Enums;
using ApiRndc.Shared.DTOs;
using MediatR;

namespace ApiRndc.Application.Commands;

/// <summary>
/// Comando para registrar cumplido de remesa o manifiesto en el RNDC (Procesos 5 y 6)
/// </summary>
public class RegistrarCumplidoCommand : IRequest<RegistrarCumplidoResult>
{
    public CumplidoDto Cumplido { get; set; } = null!;
    public TransactionType TipoCumplido { get; set; } // CumplidoRemesa o CumplidoManifiesto
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Resultado del registro de cumplido
/// </summary>
public class RegistrarCumplidoResult
{
    public bool Success { get; set; }
    public string? IngresoId { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid TransactionId { get; set; }
    public string? Consecutivo { get; set; }
    public TransactionType TipoCumplido { get; set; }
}
