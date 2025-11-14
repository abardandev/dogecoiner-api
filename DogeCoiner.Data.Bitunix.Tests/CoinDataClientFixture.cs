using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DogeCoiner.Data.Bitunix.Tests
{
    public class CoinDataFakeClientFixture
    {
        private ServiceProvider _provider;

        public IConfiguration Config { get; }
        public Mock<HttpMessageHandler> Api { get; }

        public ICoinDataClient CoinDataClient
        {
            get
            {
                return _provider.GetService<ICoinDataClient>();
            }
        }

        public CoinDataFakeClientFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.Configure<BitunixApiSettings>(Config.GetSection("ApiSettings"));

            Api = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(Api.Object);
            services.AddSingleton(httpClient);

            services.AddScoped<ICoinDataClient, BitunixDataClient>();

            _provider = services.BuildServiceProvider();
        }
    }

    public class CoinDataRealClientFixture
    {
        private ServiceProvider _provider;

        public IConfiguration Config { get; }

        public ICoinDataClient CoinDataClient
        {
            get
            {
                return _provider.GetService<ICoinDataClient>();
            }
        }

        public CoinDataRealClientFixture()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.Configure<BitunixApiSettings>(Config.GetSection("ApiSettings"));

            services.AddHttpClient();

            services.AddScoped<ICoinDataClient, BitunixDataClient>();

            _provider = services.BuildServiceProvider();
        }
    }
}
