namespace DogeCoiner.Data.Dtos
{
    public class User
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Picture { get; set; }
        public string? ProviderSub { get; set; }
        public string? ProviderName { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }

        public ICollection<Portfolio> Portfolios { get; set; }
    }
}
