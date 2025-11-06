using DogeCoiner.Data.Dtos;

namespace DogeCoiner.Data.Bitunix.Dtos
{
    public static class KLineApiItemExtensions
    {
        public static KLine ToKLine(this KLineApiItem item, string interval)
        {
            return new KLine
            {
                Symbol = item.symbol,
                Interval = interval,
                Timestamp = item.ts,
                OpenPrice = decimal.TryParse(item.open, out decimal open) ? open : 0,
                HighPrice = decimal.TryParse(item.high, out decimal high) ? high : 0,
                LowPrice = decimal.TryParse(item.low, out decimal low) ? low : 0,
                ClosePrice = decimal.TryParse(item.close, out decimal close) ? close : 0,
                Volume = decimal.TryParse(item.volume, out decimal volume) ? volume : 0
            };
        }
    }
}
