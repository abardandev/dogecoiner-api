using System.Security.Claims;

namespace DogeCoiner.Data.Auth;

/// <summary>
/// Represents an authenticated user from NextAuth JWE token claims.
/// Provides strongly-typed access to user information.
/// </summary>
public class AuthenticatedUser : IAuthUser
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
    /// Auth provider's user unique id, ex: google's sub
    /// </summary>
    public string ProviderSub => _principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? _principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? string.Empty;

    /// <summary>
    /// Auth provider name, ex: google
    /// </summary>
    public string Provider => _principal.FindFirst("provider")?.Value
        ?? _principal.FindFirst("provider")?.Value
        ?? string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName => _principal.FindFirst("firstname")?.Value
        ?? _principal.FindFirst("firstname")?.Value
        ?? string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName => _principal.FindFirst("lastname")?.Value
        ?? _principal.FindFirst("lastname")?.Value
        ?? string.Empty;

    /// <summary>
    /// Issued At timestamp (Unix timestamp)
    /// </summary>
    public long Iat
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
    public DateTimeOffset IssuedAtDateUtc => DateTimeOffset.FromUnixTimeSeconds(Iat);

    /// <summary>
    /// Expiration timestamp (Unix timestamp)
    /// </summary>
    public long Exp
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
    public DateTimeOffset ExpirationDateUtc => DateTimeOffset.FromUnixTimeSeconds(Exp);

    /// <summary>
    /// Whether the token has expired
    /// </summary>
    public bool IsExpired => DateTimeOffset.UtcNow > ExpirationDateUtc;

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
