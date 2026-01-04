using DogeCoiner.Data.Auth;
using DogeCoiner.WebApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace DogeCoiner.WebApi.Services;

public class JweDecryptionService : IJweDecryptionService
{
    private readonly JweSettings _jweSettings;
    private readonly ILogger<JweDecryptionService> _logger;
    private readonly byte[] _decryptionKey;

    public JweDecryptionService(
        IOptions<JweSettings> jweSettings,
        ILogger<JweDecryptionService> logger)
    {
        _jweSettings = jweSettings.Value;
        _logger = logger;

        // Derive the decryption key based on configuration
        _decryptionKey = Base64UrlEncoder.DecodeBytes(_jweSettings.EncryptionKey);
    }

    public AuthUserJwt DecryptAndValidate(string jweToken)
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
                throw new SecurityTokenException("Failed to deserialize JWT payload");
            }

            ValidateExpiration(user);

            // Log successful decryption
            _logger.LogInformation("Successfully decrypted and validated JWE token for subject: {Subject}",
                user.Sub);

            return user;
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
}
