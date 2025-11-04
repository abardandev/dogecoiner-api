using FluentAssertions;
using System.Data;

namespace DataImport.Tests
{
    public class CoinDataClientTests : IClassFixture<CoinDataClientFixture>
    {
        private readonly CoinDataClientFixture _fixture;

        public CoinDataClientTests(CoinDataClientFixture fixture)
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
        public async Task GetWeeklyData(
            string symbol, string interval, long? endTime, int? limit, string testFileName)
        {
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
}
