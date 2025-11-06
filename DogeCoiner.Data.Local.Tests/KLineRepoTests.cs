using DogeCoiner.Data.Bitunix.Tests;
using DogeCoiner.Data.Local.Dtos;
using System.Text.Json;

namespace DogeCoiner.Data.Local.Tests
{
    public class KLineRepoTests : IClassFixture<KLineRepoTestFixture>
    {
        private readonly KLineRepoTestFixture _fixture;

        public KLineRepoTests(KLineRepoTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("BTCUSDT", "TestData//BTCUSDT-W.json")]
        [InlineData("ETHUSDT", "TestData//ETHUSDT-W.json")]
        [InlineData("SOLUSDT", "TestData//SOLUSDT-W.json")]
        [InlineData("XRPUSDT", "TestData//XRPUSDT-W.json")]
        [InlineData("DOGEUSDT", "TestData//DOGEUSDT-W.json")]
        [InlineData("ADAUSDT", "TestData//ADAUSDT-W.json")]
        [InlineData("SHIBUSDT", "TestData//SHIBUSDT-W.json")]
        public void SaveKLineTests(string symbol, string fileName)
        {
            var json = File.ReadAllText(fileName);
            var items = JsonSerializer.Deserialize<KLine[]>(json);

            _fixture.KLineRepo.Save(items);
        }
    }
}
