using Microsoft.Extensions.Configuration;

namespace DogeCoiner.Data.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDogeCoinerConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DogeCoinerDb")
            ?? throw new InvalidOperationException(
                "Connection string 'DogeCoinerDb' not found. " +
                "Ensure it's set in appsettings.json, user secrets, or environment variables.");
    }
}
