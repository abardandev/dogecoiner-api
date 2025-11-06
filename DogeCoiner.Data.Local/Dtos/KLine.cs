namespace DogeCoiner.Data.Local.Dtos
{
    public interface IKLineKey
    {
        long ID { get; set; }
        string Symbol { get; set; }
        string Interval { get; set; }
        DateTime Timestamp { get; set; }
    }

    public class KLineKey : IKLineKey
    {
        public long ID { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class KLine : IKLineKey
    {
        public long ID { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Volume { get; set; }
    }
}
