using DogeCoiner.Data.Auth;

namespace DogeCoiner.WebApi.Services;

public interface IJweDecryptionService
{
    AuthUserJwt DecryptAndValidate(string jweToken);
}
