using DogeCoiner.Data.Bitunix.Dtos;
using DogeCoiner.Data.Bitunix.Tests;
using System.Text.Json;

namespace DogeCoiner.Data.DAL.Tests
{
    public class KLineRepoTests : IClassFixture<KLineRepoTestFixture>
    {
        private readonly KLineRepoTestFixture _fixture;

        public KLineRepoTests(KLineRepoTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("W", "TestData//BTCUSDT-W.json")]
        [InlineData("W", "TestData//ETHUSDT-W.json")]
        [InlineData("W", "TestData//SOLUSDT-W.json")]
        [InlineData("W", "TestData//XRPUSDT-W.json")]
        [InlineData("W", "TestData//DOGEUSDT-W.json")]
        [InlineData("W", "TestData//ADAUSDT-W.json")]
        [InlineData("W", "TestData//SHIBUSDT-W.json")]

        [InlineData("D", "TestData//BTCUSDT-D-2025.json")]
        [InlineData("D", "TestData//BTCUSDT-D-2024.json")]
        [InlineData("D", "TestData//BTCUSDT-D-2023.json")]
        [InlineData("D", "TestData//BTCUSDT-D-2022.json")]

        [InlineData("D", "TestData//ETHUSDT-D-2025.json")]
        [InlineData("D", "TestData//ETHUSDT-D-2024.json")]
        [InlineData("D", "TestData//ETHUSDT-D-2023.json")]
        [InlineData("D", "TestData//ETHUSDT-D-2022.json")]

        [InlineData("D", "TestData//SOLUSDT-D-2025.json")]
        [InlineData("D", "TestData//SOLUSDT-D-2024.json")]
        [InlineData("D", "TestData//SOLUSDT-D-2023.json")]

        [InlineData("D", "TestData//XRPUSDT-D-2025.json")]
        [InlineData("D", "TestData//XRPUSDT-D-2024.json")]
        [InlineData("D", "TestData//XRPUSDT-D-2023.json")]
        [InlineData("D", "TestData//XRPUSDT-D-2022.json")]

        [InlineData("D", "TestData//DOGEUSDT-D-2025.json")]
        [InlineData("D", "TestData//DOGEUSDT-D-2024.json")]
        [InlineData("D", "TestData//DOGEUSDT-D-2023.json")]
        [InlineData("D", "TestData//DOGEUSDT-D-2022.json")]

        [InlineData("D", "TestData//ADAUSDT-D-2025.json")]
        [InlineData("D", "TestData//ADAUSDT-D-2024.json")]
        [InlineData("D", "TestData//ADAUSDT-D-2023.json")]
        [InlineData("D", "TestData//ADAUSDT-D-2022.json")]

        [InlineData("D", "TestData//SHIBUSDT-D-2025.json")]
        [InlineData("D", "TestData//SHIBUSDT-D-2024.json")]
        [InlineData ("D", "TestData//SHIBUSDT-D-2023.json")]
        [InlineData("D", "TestData//SHIBUSDT-D-2022.json")]
        public void SaveKLineTests(string interval, string fileName)
        {
            var json = File.ReadAllText(fileName);
            var bitunixResponse = JsonSerializer.Deserialize<KLineApiResponse>(json);
            var items = bitunixResponse.data.Select(o => o.ToKLine(interval)).ToArray();

            _fixture.KLineRepo.Save(items);
        }
    }
}
