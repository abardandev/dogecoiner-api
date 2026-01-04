using System.ComponentModel.DataAnnotations.Schema;

namespace DogeCoiner.Data.Dtos
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public long PortfolioId { get; set; }
        public string Symbol { get; set; }
        public string TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime TimestampUtc { get; set; }

        [ForeignKey(nameof(PortfolioId))]
        public Portfolio Portfolio { get; set; }
    }
}
