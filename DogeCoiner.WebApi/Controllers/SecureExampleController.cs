using DogeCoiner.Data.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogeCoiner.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SecureExampleController : ControllerBase
{
    private readonly ILogger<SecureExampleController> _logger;

    public SecureExampleController(ILogger<SecureExampleController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Example of a protected endpoint using the AuthenticatedUser wrapper.
    /// Returns the authenticated user's information.
    /// </summary>
    [HttpGet("user-info")]
    public IActionResult GetUserInfo()
    {
        // Get the authenticated user using the extension method
        var user = User.ToAuthenticatedUser();

        _logger.LogInformation("User {Sub} accessed user info endpoint",
            user.ProviderSub);

        return Ok(new
        {
            user.ProviderSub,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Picture   
        });
    }

    /// <summary>
    /// Alternative: Access user info without the wrapper (original approach)
    /// </summary>
    [HttpGet("user-info-raw")]
    public IActionResult GetUserInfoRaw()
    {
        // Extract claims from the authenticated user directly
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        // Get all claims for demonstration
        var allClaims = User.Claims.Select(c => new
        {
            Type = c.Type,
            Value = c.Value
        });

        _logger.LogInformation("User {UserId} accessed user info endpoint", userId);

        return Ok(new
        {
            UserId = userId,
            Username = username,
            Email = email,
            AllClaims = allClaims
        });
    }

    /// <summary>
    /// Example of a protected endpoint with role-based authorization.
    /// Uncomment and modify based on your role claim structure.
    /// </summary>
    /*
    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminData()
    {
        return Ok(new { Message = "This is admin-only data" });
    }
    */

    /// <summary>
    /// Example of a protected POST endpoint using AuthenticatedUser wrapper.
    /// </summary>
    [HttpPost("secure-action")]
    public IActionResult PerformSecureAction([FromBody] SecureActionRequest request)
    {
        var user = User.ToAuthenticatedUser();

        _logger.LogInformation("User {Sub} performed secure action: {Action}",
            user.ProviderSub, user.Name, request.ActionName);

        return Ok(new
        {
            Success = true,
            Message = $"Action '{request.ActionName}' completed successfully by {user.Name}",
            user.ProviderSub,
            UserEmail = user.Email
        });
    }

    /// <summary>
    /// Example of an endpoint that allows anonymous access even though
    /// the controller requires authentication.
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublicData()
    {
        return Ok(new
        {
            Message = "This endpoint is publicly accessible",
            Timestamp = DateTime.UtcNow
        });
    }
}

public class SecureActionRequest
{
    public string ActionName { get; set; } = string.Empty;
    public Dictionary<string, string>? Parameters { get; set; }
}
