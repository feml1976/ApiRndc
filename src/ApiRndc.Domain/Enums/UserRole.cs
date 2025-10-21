namespace ApiRndc.Domain.Enums;

/// <summary>
/// Roles de usuario en el sistema
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Administrador del sistema
    /// </summary>
    Administrador = 1,

    /// <summary>
    /// Operador que puede crear y enviar transacciones
    /// </summary>
    Operador = 2,

    /// <summary>
    /// Usuario de solo consulta
    /// </summary>
    Consulta = 3
}
