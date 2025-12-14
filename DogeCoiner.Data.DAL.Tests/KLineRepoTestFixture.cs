using DogeCoiner.Data.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogeCoiner.Data.Bitunix.Tests
{
    public class KLineRepoTestFixture
    {
        private ServiceProvider _provider;

        public IConfiguration Config { get; }

        public IKLineRepo KLineRepo
        {
            get
            {
                return _provider.GetService<IKLineRepo>();
            }
        }

        public KLineRepoTestFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.Configure<DogeCoinerDataSettings>(Config.GetSection("DbSettings"));

            services.AddScoped<IKLineRepo, KLineRepo>();

            _provider = services.BuildServiceProvider();
        }
    }
}
