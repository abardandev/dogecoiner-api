using DogeCoiner.Data.Bitunix.Dtos;
using FluentAssertions;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace DogeCoiner.Data.Import.Tests
{
    public class KLineApiItemExtensionsTests
    {
        [Theory]
        [InlineData("BTCUSDT", "TestData//BTCUSDT-W-500.json")]
        [InlineData("ETHUSDT", "TestData//ETHUSDT-W-500.json")]
        [InlineData("SOLUSDT", "TestData//SOLUSDT-W-500.json")]
        [InlineData("XRPUSDT", "TestData//XRPUSDT-W-500.json")]
        [InlineData("DOGEUSDT", "TestData//DOGEUSDT-W-500.json")]
        [InlineData("ADAUSDT", "TestData//ADAUSDT-W-500.json")]
        [InlineData("SHIBUSDT", "TestData//SHIBUSDT-W-500.json")]
        public void ToKLineTest(string symbol, string testFileName)
        {
            var payload = File.ReadAllText(testFileName);
            var klineResponse = JsonSerializer.Deserialize<KLineApiResponse>(payload);
            var klines = klineResponse.data
                .Select(o => o.ToKLine("W"))
                .ToArray();

            klines.Should().NotBeEmpty();
            klines.All(o => o.Symbol == symbol).Should().BeTrue();
            klines.All(o => o.Interval == "W").Should().BeTrue();
            klines.All(o => o.Timestamp > DateTime.MinValue).Should().BeTrue();
            klines.All(o => o.OpenPrice != 0).Should().BeTrue();
            klines.All(o => o.HighPrice != 0).Should().BeTrue();
            klines.All(o => o.LowPrice != 0).Should().BeTrue();
            klines.All(o => o.ClosePrice != 0).Should().BeTrue();
            klines.Any(o => o.Volume > 0).Should().BeTrue();
        }
    }
}
