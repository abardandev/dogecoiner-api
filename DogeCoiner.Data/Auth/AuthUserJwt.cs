using DogeCoiner.Data.Dtos;
using System.Security.Claims;

namespace DogeCoiner.Data.Auth;

public interface IAuthUser
{
    string Name { get; }
    string Email { get; }
    string Picture { get; }
    string ProviderSub { get; }
    string Provider { get; }
    string FirstName { get; }
    string LastName { get; }
    long Iat { get; }
    long Exp { get; }
}

public class AuthUserJwt : IAuthUser
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string ProviderSub { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public long Iat { get; set; }
    public long Exp { get; set; }
}

public static class AuthUserJwtExtensions
{
    public static User ToUser(this AuthUserJwt user)
    {
        return new User
        {
            Email = user.Email,
            Picture = user.Picture,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProviderName = user.Provider,
            ProviderSub = user.ProviderSub
        };
    }

    public static AuthenticatedUser ToAuthenticatedUser(this AuthUserJwt user)
    {
        return new AuthenticatedUser(ToClaimsPrincipal(user));
    }

    public static ClaimsPrincipal ToClaimsPrincipal(this AuthUserJwt user)
    {
        var claims = GetClaims(user);
        var identity = new ClaimsIdentity(claims, "JweBearer");
        var principal = new ClaimsPrincipal(identity);
        return principal;
    }

    private static List<Claim> GetClaims(AuthUserJwt user)
    {
        // Create claims from the payload with proper claim type mapping
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("picture", user.Picture),
            new Claim(ClaimTypes.NameIdentifier, user.ProviderSub),
            new Claim("provider", user.Provider),
            new Claim("firstname", user.FirstName),
            new Claim("lastname", user.LastName),
            new Claim("iat", user.Iat.ToString()),
            new Claim("exp", user.Exp.ToString())
        };
    }
}