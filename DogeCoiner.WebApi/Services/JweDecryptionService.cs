using DogeCoiner.WebApi.Configuration;
using DogeCoiner.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace DogeCoiner.WebApi.Services;

public class JweDecryptionService : IJweDecryptionService
{
    private readonly JweSettings _jweSettings;
    private readonly ILogger<JweDecryptionService> _logger;
    private readonly JsonWebTokenHandler _tokenHandler;
    private readonly byte[] _decryptionKey;

    public JweDecryptionService(
        IOptions<JweSettings> jweSettings,
        ILogger<JweDecryptionService> logger)
    {
        _jweSettings = jweSettings.Value;
        _logger = logger;
        _tokenHandler = new JsonWebTokenHandler();

        // Derive the decryption key based on configuration
        _decryptionKey = Base64UrlEncoder.DecodeBytes(_jweSettings.EncryptionKey);
    }

    public async Task<ClaimsPrincipal> DecryptAndValidateAsync(string jweToken)
    {
        try
        {
            // Validate the JWE token format
            if (string.IsNullOrWhiteSpace(jweToken))
            {
                throw new SecurityTokenException("JWE token is null or empty");
            }

            var decryptedJwt = Jose.JWT.Decrypt(jweToken, _decryptionKey);

            var user = JsonSerializer.Deserialize<AuthUserJwt>(decryptedJwt, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (user == null)
            {
                throw new SecurityTokenException("JWE token is null or empty");
            }

            ValidateExpiration(user);

            var principal = BuildPrincipal(user);

            // Log successful decryption
            _logger.LogInformation("Successfully decrypted and validated JWE token for subject: {Subject}",
                principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown");

            return await Task.FromResult(principal);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning(ex, "JWE token has expired");
            throw;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(ex, "JWE token validation failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during JWE token decryption");
            throw new SecurityTokenException("Failed to decrypt and validate JWE token", ex);
        }
    }

    private void ValidateExpiration(AuthUserJwt user)
    {
        // Validate expiration if required
        if (_jweSettings.ValidateLifetime)
        {
            var expDate = DateTimeOffset.FromUnixTimeSeconds(user.Exp);
            var nowMinusSkew = DateTimeOffset.UtcNow - TimeSpan.FromSeconds(_jweSettings.ClockSkewSeconds);

            if (expDate < nowMinusSkew)
            {
                throw new SecurityTokenExpiredException($"Token expired at {expDate}");
            }
        }
    }

    private ClaimsPrincipal BuildPrincipal(AuthUserJwt user)
    {
        var claims = user.GetClaims();
        
        var identity = new ClaimsIdentity(claims, "JweBearer");
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
}
