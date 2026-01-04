using DogeCoiner.Data.Auth;
using DogeCoiner.Data.DAL.Repos.Users;
using DogeCoiner.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Encodings.Web;

namespace DogeCoiner.WebApi.Authentication;

public class JweAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IJweDecryptionService _jweDecryptionService;
    private readonly IUsersRepo _usersRepo;

    public JweAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IJweDecryptionService jweDecryptionService,
        IUsersRepo usersRepo)
        : base(options, logger, encoder)
    {
        _jweDecryptionService = jweDecryptionService;
        _usersRepo = usersRepo;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // skip building token if endpoint allows anonymous
        var endpoint = Context.GetEndpoint();
        if(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return AuthenticateResult.NoResult();
        }

        // Check if Authorization header exists
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.NoResult();
        }

        string? authorizationHeader = Request.Headers.Authorization.ToString();

        // Check if the header has the Bearer scheme
        if (string.IsNullOrWhiteSpace(authorizationHeader)
            || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        // Extract the token
        string jweToken = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrWhiteSpace(jweToken))
        {
            return AuthenticateResult.Fail("Invalid token format");
        }

        try
        {
            // Step 1: Decrypt and validate the JWE token
            var authUser = _jweDecryptionService.DecryptAndValidate(jweToken);

            // Step 2: Ensure user exists (auto-register if new user)
            if (!_usersRepo.UserExists(authUser.Email))
            {
                Logger.LogInformation("Auto-registering new user: {Email}", authUser.Email);
                await _usersRepo.SaveAsync(authUser.ToUser());
            }

            // Step 3: Build ClaimsPrincipal
            var principal = authUser.ToClaimsPrincipal();

            // Create the authentication ticket
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Logger.LogInformation("Successfully authenticated user: {Sub}", authUser.ProviderSub);

            return AuthenticateResult.Success(ticket);
        }
        catch (SecurityTokenExpiredException)
        {
            Logger.LogWarning("Token has expired");
            return AuthenticateResult.Fail("Token has expired");
        }
        catch (SecurityTokenException ex)
        {
            Logger.LogWarning(ex, "Token validation failed");
            return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error during authentication");
            return AuthenticateResult.Fail("An error occurred during authentication");
        }
    }
}
