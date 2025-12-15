namespace DogeCoiner.Data.Dtos
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsRegistered { get; set; }

        public ICollection<Portfolio> Portfolios { get; set; }
    }
}
