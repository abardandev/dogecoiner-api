using DogeCoiner.Data.DAL;
using DogeCoiner.Data.DAL.Repos.Users;
using DogeCoiner.Data.DAL.Repos.Users.Strategies;
using DogeCoiner.Data.Extensions;
using DogeCoiner.WebApi.Authentication;
using DogeCoiner.WebApi.Configuration;
using DogeCoiner.WebApi.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DogeCoiner.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDogeCoiner(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbConn = configuration.GetDogeCoinerConnectionString();

        services.AddDbContext<CoinDataDbContext>(options =>
            options.UseSqlServer(dbConn));

        services.Configure<DogeCoinerSettings>(config =>
        {
            config.ConnectionString = dbConn;
        });

        // register services
        services.AddScoped<ISaveUsers, DapperSaveUsers>();
        services.AddScoped<IUsersRepo, UsersRepo>();

        // add hangfire
        services.AddHangfire(configuration => 
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(dbConn));
        
        services.AddHangfireServer();

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
