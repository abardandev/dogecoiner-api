using System.Security.Claims;

namespace DogeCoiner.WebApi.Models;

public class AuthUserJwt
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string Sub { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public long Iat { get; set; }
    public long Exp { get; set; }
    public string Jti { get; set; } = string.Empty;

    public List<Claim> GetClaims()
    {
        // Create claims from the payload with proper claim type mapping
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, Name),
            new Claim(ClaimTypes.Email, Email),
            new Claim("picture", Picture),
            new Claim(ClaimTypes.NameIdentifier, Sub),
            new Claim("userId", UserId),
            new Claim("iat", Iat.ToString()),
            new Claim("exp", Exp.ToString()),
            new Claim("jti", Jti)
        };
    }
}