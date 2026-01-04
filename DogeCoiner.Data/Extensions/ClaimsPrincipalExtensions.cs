using DogeCoiner.Data.Auth;
using System.Security.Claims;

namespace DogeCoiner.Data.Extensions;

/// <summary>
/// Extension methods for ClaimsPrincipal to easily access authenticated user information
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Converts a ClaimsPrincipal to an AuthenticatedUser wrapper
    /// </summary>
    public static AuthenticatedUser ToAuthenticatedUser(this ClaimsPrincipal principal)
    {
        return new AuthenticatedUser(principal);
    }
}