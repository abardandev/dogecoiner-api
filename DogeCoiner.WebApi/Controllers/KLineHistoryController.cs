using DogeCoiner.Data.Dtos;
using DogeCoiner.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DogeCoiner.Data.WebApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class KLineHistoryController : ControllerBase
    {
        private CoinDataDbContext _db;
        private readonly ILogger<WeatherForecastController> _logger;

        public KLineHistoryController(CoinDataDbContext db, ILogger<WeatherForecastController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet("klinehistory")]
        public IEnumerable<KLine> Get(
            [FromQuery] string symbol,
            [FromQuery] string interval)
        {
            return _db.KLines
                .Where(o => o.Symbol == symbol && o.Interval == interval)
                .ToArray();
        }

        [HttpGet("klinehistory/linedata")]
        public IEnumerable<dynamic> GetLineData(
            [FromQuery] string symbol,
            [FromQuery] string interval)
        {
            return _db.KLines
                .Where(o => o.Symbol == symbol && o.Interval == interval)
                .OrderBy(o => o.TimestampUtc)
                .Select(o => new
                {
                    time = o.TimestampUtc.ToString("yyyy-MM-dd"),
                    value = o.ClosePrice
                })
                .ToArray();
        }
    }
}
