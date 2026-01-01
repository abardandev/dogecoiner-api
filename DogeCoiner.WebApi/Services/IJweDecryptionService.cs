using System.Security.Claims;

namespace DogeCoiner.WebApi.Services;

public interface IJweDecryptionService
{
    Task<ClaimsPrincipal> DecryptAndValidateAsync(string jweToken);
}
