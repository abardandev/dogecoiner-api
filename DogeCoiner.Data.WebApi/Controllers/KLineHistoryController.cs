using DogeCoiner.Data.Dtos;
using DogeCoiner.Data.Local;
using Microsoft.AspNetCore.Mvc;

namespace DogeCoiner.Data.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KLineHistoryController : ControllerBase
    {
        private CoinDataDbContext _ctx;
        private readonly ILogger<WeatherForecastController> _logger;

        public KLineHistoryController(CoinDataDbContext ctx, ILogger<WeatherForecastController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<KLine> Get(
            [FromQuery] string symbol,
            [FromQuery] string interval)
        {
            return _ctx.KLines
                .Where(o => o.Symbol == symbol && o.Interval == interval)
                .ToArray();
        }

        [HttpGet("LineData")]
        public IEnumerable<dynamic> GetLineData(
            [FromQuery] string symbol,
            [FromQuery] string interval)
        {
            return _ctx.KLines
                .Where(o => o.Symbol == symbol && o.Interval == interval)
                .OrderBy(o => o.Timestamp)
                .Select(o => new
                {
                    time = o.Timestamp.ToString("yyyy-MM-dd"),
                    value = o.ClosePrice
                })
                .ToArray();
        }
    }
}
