namespace DogeCoiner.Data.DAL
{
    public class SavePortfolioDbResult
    {
        public string Action { get; set; }
        public int PortfolioId { get; set; }
        public int UserId { get; set; }
        public string PortfolioName { get; set; }
    }
}
