using DogeCoiner.Data.DAL;
using DogeCoiner.Data.DAL.Repos.KLines;
using DogeCoiner.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogeCoiner.Data.Bitunix.Tests
{
    public class KLineRepoTestFixture
    {
        private ServiceProvider _provider;

        public IConfiguration Config { get; }

        public IKLinesRepo KLineRepo
        {
            get
            {
                return _provider.GetService<IKLinesRepo>();
            }
        }

        public KLineRepoTestFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.Configure<DogeCoinerSettings>(config => {
                config.ConnectionString = Config.GetDogeCoinerConnectionString();
            });

            services.AddScoped<IKLinesRepo, KLinesRepo>();

            _provider = services.BuildServiceProvider();
        }
    }
}
