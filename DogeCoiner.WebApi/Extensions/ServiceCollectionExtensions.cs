using DogeCoiner.Data.DAL;
using DogeCoiner.Data.DAL.Repos.Users;
using DogeCoiner.Data.DAL.Repos.Users.Strategies;
using DogeCoiner.Data.Extensions;
using DogeCoiner.WebApi.Authentication;
using DogeCoiner.WebApi.Configuration;
using DogeCoiner.WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DogeCoiner.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDogeCoiner(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get connection string - single source of truth
        var dbConn = configuration.GetDogeCoinerConnectionString();

        // Database context
        services.AddDbContext<CoinDataDbContext>(options =>
            options.UseSqlServer(dbConn));

        // Configure DogeCoiner settings with the same connection string
        services.Configure<DogeCoinerSettings>(config =>
        {
            config.ConnectionString = dbConn;
        });

        // Register repositories
        services.AddScoped<ISaveUsers, DapperSaveUsers>();
        services.AddScoped<IUsersRepo, UsersRepo>();

        return services;
    }

    public static IServiceCollection AddDogeCoinerSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure JWE settings
        services.Configure<JweSettings>(
            configuration.GetSection(JweSettings.SectionName));

        // Register services
        services.AddScoped<IJweDecryptionService, JweDecryptionService>();

        // Configure authentication with JWE handler
        services.AddAuthentication("JweBearer")
            .AddScheme<AuthenticationSchemeOptions, JweAuthenticationHandler>("JweBearer", null);

        services.AddAuthorization();

        return services;
    }
}
