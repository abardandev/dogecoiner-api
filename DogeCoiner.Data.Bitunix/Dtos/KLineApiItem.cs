namespace DogeCoiner.Data.Bitunix.Dtos
{
    public class KLineApiItem
    {
        public string symbol { get; set; }

        public DateTime ts { get; set; }

        public string open {  get; set; }

        public string high { get; set; }

        public string low { get; set; }

        public string close { get; set; }

        public string volume { get; set; }
    }
}
