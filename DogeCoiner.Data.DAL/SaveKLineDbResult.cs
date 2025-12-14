namespace DogeCoiner.Data.DAL
{
    public class SaveKLineDbResult
    {
        public string Action { get; set; }
        public int ID { get; set; }
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
