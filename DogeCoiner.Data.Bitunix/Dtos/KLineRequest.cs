namespace DogeCoiner.Data.Bitunix.Dtos
{
    public class KLineRequest
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public long? EndTime { get; set; }
        public int? Limit { get; set; }

        public KLineRequest(string symbol, string interval, long? endTime = null, int? limit = null)
        {
            Symbol = symbol;
            Interval = interval;
            EndTime = endTime;
            Limit = limit;
        }
    }
}
