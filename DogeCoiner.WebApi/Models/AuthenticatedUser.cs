using System.Security.Claims;

namespace DogeCoiner.WebApi.Models;

/// <summary>
/// Represents an authenticated user from NextAuth JWE token claims.
/// Provides strongly-typed access to user information.
/// </summary>
public class AuthenticatedUser
{
    private readonly ClaimsPrincipal _principal;

    public AuthenticatedUser(ClaimsPrincipal principal)
    {
        _principal = principal ?? throw new ArgumentNullException(nameof(principal));
    }

    /// <summary>
    /// User's full name
    /// </summary>
    public string Name => _principal.FindFirst(ClaimTypes.Name)?.Value
        ?? _principal.FindFirst("name")?.Value
        ?? string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email => _principal.FindFirst(ClaimTypes.Email)?.Value
        ?? _principal.FindFirst("email")?.Value
        ?? string.Empty;

    /// <summary>
    /// URL to user's profile picture
    /// </summary>
    public string Picture => _principal.FindFirst("picture")?.Value ?? string.Empty;

    /// <summary>
    /// User's unique identifier (subject)
    /// </summary>
    public string Sub => _principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _principal.FindFirst("sub")?.Value
        ?? string.Empty;

    /// <summary>
    /// User ID (same as Sub for NextAuth)
    /// </summary>
    public string UserId => _principal.FindFirst("userId")?.Value
        ?? Sub;

    /// <summary>
    /// Issued At timestamp (Unix timestamp)
    /// </summary>
    public long IssuedAt
    {
        get
        {
            var iatClaim = _principal.FindFirst("iat")?.Value;
            return long.TryParse(iatClaim, out var iat) ? iat : 0;
        }
    }

    /// <summary>
    /// Issued At as DateTimeOffset
    /// </summary>
    public DateTimeOffset IssuedAtDate => DateTimeOffset.FromUnixTimeSeconds(IssuedAt);

    /// <summary>
    /// Expiration timestamp (Unix timestamp)
    /// </summary>
    public long Expiration
    {
        get
        {
            var expClaim = _principal.FindFirst("exp")?.Value;
            return long.TryParse(expClaim, out var exp) ? exp : 0;
        }
    }

    /// <summary>
    /// Expiration as DateTimeOffset
    /// </summary>
    public DateTimeOffset ExpirationDate => DateTimeOffset.FromUnixTimeSeconds(Expiration);

    /// <summary>
    /// JWT ID (unique identifier for this token)
    /// </summary>
    public string Jti => _principal.FindFirst("jti")?.Value ?? string.Empty;

    /// <summary>
    /// Whether the token has expired
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow > ExpirationDate;

    /// <summary>
    /// Whether the user is authenticated
    /// </summary>
    public bool IsAuthenticated => _principal.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// Gets the underlying ClaimsPrincipal
    /// </summary>
    public ClaimsPrincipal Principal => _principal;

    /// <summary>
    /// Gets a specific claim value by claim type
    /// </summary>
    public string? GetClaimValue(string claimType)
    {
        return _principal.FindFirst(claimType)?.Value;
    }

    /// <summary>
    /// Gets all claims
    /// </summary>
    public IEnumerable<Claim> GetAllClaims()
    {
        return _principal.Claims;
    }

    /// <summary>
    /// Creates an AuthenticatedUser from the current HttpContext user
    /// </summary>
    public static AuthenticatedUser FromPrincipal(ClaimsPrincipal principal)
    {
        return new AuthenticatedUser(principal);
    }
}
