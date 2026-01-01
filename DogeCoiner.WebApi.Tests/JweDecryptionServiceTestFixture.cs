using DogeCoiner.WebApi.Configuration;
using DogeCoiner.WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DogeCoiner.WebApi.Tests
{
    public class JweDecryptionServiceTestFixture
    {
        private ServiceProvider _provider;

        public IConfiguration Config { get; }

        public IJweDecryptionService JweService
        {
            get
            {
                return _provider.GetService<IJweDecryptionService>();
            }
        }

        public JweDecryptionServiceTestFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        internal void Configure(ITestOutputHelper outputHelper)
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddXUnit(outputHelper); // Add the xUnit logging provider
                builder.SetMinimumLevel(LogLevel.Information); // Optional level setting
            });

            services.Configure<JweSettings>(Config.GetSection("JweSettings"));

            services.AddScoped<IJweDecryptionService, JweDecryptionService>();

            _provider = services.BuildServiceProvider();
        }
    }
}
