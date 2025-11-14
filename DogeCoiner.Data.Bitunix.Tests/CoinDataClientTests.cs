using DogeCoiner.Data.Bitunix.Dtos;
using FluentAssertions;
using System.Data;
using System.Text.Json;

namespace DogeCoiner.Data.Bitunix.Tests
{
    public class CoinDataFakeClientTests : IClassFixture<CoinDataFakeClientFixture>
    {
        private readonly CoinDataFakeClientFixture _fixture;

        public CoinDataFakeClientTests(CoinDataFakeClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("BTCUSDT", "W", null, 500, "TestData//BTCUSDT-W-500.json")]
        [InlineData("ETHUSDT", "W", null, 500, "TestData//ETHUSDT-W-500.json")]
        [InlineData("SOLUSDT", "W", null, 500, "TestData//SOLUSDT-W-500.json")]
        [InlineData("XRPUSDT", "W", null, 500, "TestData//XRPUSDT-W-500.json")]
        [InlineData("DOGEUSDT", "W", null, 500, "TestData//DOGEUSDT-W-500.json")]
        [InlineData("ADAUSDT", "W", null, 500, "TestData//ADAUSDT-W-500.json")]
        [InlineData("SHIBUSDT", "W", null, 500, "TestData//SHIBUSDT-W-500.json")]

        [InlineData("BTCUSDT", "D", "2025-12-31", 365, "TestData//BTCUSDT-D-2025.json")]
        [InlineData("BTCUSDT", "D", "2024-12-31", 365, "TestData//BTCUSDT-D-2024.json")]
        [InlineData("BTCUSDT", "D", "2023-12-31", 365, "TestData//BTCUSDT-D-2023.json")]
        [InlineData("BTCUSDT", "D", "2022-12-31", 365, "TestData//BTCUSDT-D-2022.json")]

        [InlineData("ETHUSDT", "D", "2025-12-31", 365, "TestData//ETHUSDT-D-2025.json")]
        [InlineData("ETHUSDT", "D", "2024-12-31", 365, "TestData//ETHUSDT-D-2024.json")]
        [InlineData("ETHUSDT", "D", "2023-12-31", 365, "TestData//ETHUSDT-D-2023.json")]
        [InlineData("ETHUSDT", "D", "2022-12-31", 365, "TestData//ETHUSDT-D-2022.json")]

        [InlineData("SOLUSDT", "D", "2025-12-31", 365, "TestData//SOLUSDT-D-2025.json")]
        [InlineData("SOLUSDT", "D", "2024-12-31", 365, "TestData//SOLUSDT-D-2024.json")]
        [InlineData("SOLUSDT", "D", "2023-12-31", 365, "TestData//SOLUSDT-D-2023.json")]

        [InlineData("XRPUSDT", "D", "2025-12-31", 365, "TestData//XRPUSDT-D-2025.json")]
        [InlineData("XRPUSDT", "D", "2024-12-31", 365, "TestData//XRPUSDT-D-2024.json")]
        [InlineData("XRPUSDT", "D", "2023-12-31", 365, "TestData//XRPUSDT-D-2023.json")]
        [InlineData("XRPUSDT", "D", "2022-12-31", 365, "TestData//XRPUSDT-D-2022.json")]

        [InlineData("DOGEUSDT", "D", "2025-12-31", 365, "TestData//DOGEUSDT-D-2025.json")]
        [InlineData("DOGEUSDT", "D", "2024-12-31", 365, "TestData//DOGEUSDT-D-2024.json")]
        [InlineData("DOGEUSDT", "D", "2023-12-31", 365, "TestData//DOGEUSDT-D-2023.json")]
        [InlineData("DOGEUSDT", "D", "2022-12-31", 365, "TestData//DOGEUSDT-D-2022.json")]

        [InlineData("ADAUSDT", "D", "2025-12-31", 365, "TestData//ADAUSDT-D-2025.json")]
        [InlineData("ADAUSDT", "D", "2024-12-31", 365, "TestData//ADAUSDT-D-2024.json")]
        [InlineData("ADAUSDT", "D", "2023-12-31", 365, "TestData//ADAUSDT-D-2023.json")]
        [InlineData("ADAUSDT", "D", "2022-12-31", 365, "TestData//ADAUSDT-D-2022.json")]

        [InlineData("SHIBUSDT", "D", "2025-12-31", 365, "TestData//SHIBUSDT-D-2025.json")]
        [InlineData("SHIBUSDT", "D", "2024-12-31", 365, "TestData//SHIBUSDT-D-2024.json")]
        [InlineData("SHIBUSDT", "D", "2023-12-31", 365, "TestData//SHIBUSDT-D-2023.json")]
        [InlineData("SHIBUSDT", "D", "2022-12-31", 365, "TestData//SHIBUSDT-D-2022.json")]
        public async Task GetWeeklyData(
            string symbol, string interval, string endTimeDate, int? limit, string testFileName)
        {
            long? endTime = endTimeDate != null ? DateTimeOffset.Parse(endTimeDate).ToUnixTimeSeconds() : null;
            var req = new KLineRequest(symbol,interval, endTime, limit);
            SetupBtcWeeklyResponse(req, testFileName);

            var res = await _fixture.CoinDataClient.GetKLineHistory(req);

            res.Should().NotBeNull();
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            res.data.Should().NotBeEmpty();
            
            var resSymbols = res.data.Select(o => o.symbol).Distinct().ToArray();

            resSymbols.Should().HaveCount(1);
            resSymbols[0].Should().Be(symbol);
        }

        private void SetupBtcWeeklyResponse(KLineRequest req, string testFileName)
        {
            // load file
            var payload = File.ReadAllText(testFileName);
            var reqUri = _fixture.CoinDataClient.BuildRequestUri(req);

            _fixture.Api
                .SetupSendAsync(HttpMethod.Get, reqUri.ToString())
                .ReturnsHttpResponseAsync(payload, System.Net.HttpStatusCode.OK);
        } 
    }

    public class CoinDataRealClientTests : IClassFixture<CoinDataRealClientFixture>
    {
        private readonly CoinDataRealClientFixture _fixture;

        public CoinDataRealClientTests(CoinDataRealClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("BTCUSDT", "D", "2025-12-31", 365, "TestData//BTCUSDT-D-2025.json")]
        [InlineData("ETHUSDT", "D", "2025-12-31", 365, "TestData//ETHUSDT-D-2025.json")]
        [InlineData("SOLUSDT", "D", "2025-12-31", 365, "TestData//SOLUSDT-D-2025.json")]
        [InlineData("XRPUSDT", "D", "2025-12-31", 365, "TestData//XRPUSDT-D-2025.json")]
        [InlineData("DOGEUSDT", "D", "2025-12-31", 365, "TestData//DOGEUSDT-D-2025.json")]
        [InlineData("ADAUSDT", "D", "2025-12-31", 365, "TestData//ADAUSDT-D-2025.json")]
        [InlineData("SHIBUSDT", "D", "2025-12-31", 365, "TestData//SHIBUSDT-D-2025.json")]
        public async Task GetDailyData(
            string symbol, string interval, string endTimeDate, int? limit, string testFileName)
        {
            long? endTime = endTimeDate != null ? DateTimeOffset.Parse(endTimeDate).ToUnixTimeSeconds() : null;
            var req = new KLineRequest(symbol, interval, endTime, limit);

            var res = await _fixture.CoinDataClient.GetKLineHistory(req);

            var json = JsonSerializer.Serialize(res);
            File.WriteAllText(testFileName, json);

            res.Should().NotBeNull();
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            res.data.Should().NotBeEmpty();

            var resSymbols = res.data.Select(o => o.symbol).Distinct().ToArray();

            resSymbols.Should().HaveCount(1);
            resSymbols[0].Should().Be(symbol);
        }
    }
}
