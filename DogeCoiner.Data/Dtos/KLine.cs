namespace DogeCoiner.Data.Dtos
{
    public interface IKLineKey
    {
        long KLineId { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        DateTime TimestampUtc { get; set; }
    }

    public class KLineKey : IKLineKey
    {
        public long KLineId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; }
    }

    public class KLine : IKLineKey
    {
        public long KLineId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
    }
}
