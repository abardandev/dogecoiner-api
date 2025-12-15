using System.ComponentModel.DataAnnotations.Schema;

namespace DogeCoiner.Data.Dtos
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public int UserId { get; set; }
        public string PortfolioName { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
