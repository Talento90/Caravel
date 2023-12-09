using System.Security.Claims;

namespace Caravel.ApplicationContext;

/// <summary>
/// Encapsulates all Application specific information about an individual operation.
/// </summary>
public record ApplicationContext(ClaimsPrincipal User);