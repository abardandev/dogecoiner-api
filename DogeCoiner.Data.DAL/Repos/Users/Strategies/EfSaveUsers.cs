using DogeCoiner.Data.Dtos;

namespace DogeCoiner.Data.DAL.Repos.Users.Strategies
{
    public class EfSaveUsers : ISaveUsers
    {
        private readonly CoinDataDbContext _db;

        public EfSaveUsers(CoinDataDbContext db)
        {
            _db = db;
        }

        public async Task SaveAsync(User[] users)
        {
            // Optimization: Single query to get all existing users by email
            var emails = users.Select(u => u.Email).ToArray();
            var existingUsers = _db.Users
                .Where(u => emails.Contains(u.Email))
                .ToDictionary(u => u.Email);

            foreach (var item in users)
            {
                if (existingUsers.TryGetValue(item.Email, out var existing))
                {
                    // Update existing user
                    existing.FirstName = item.FirstName;
                    existing.LastName = item.LastName;
                    existing.Picture = item.Picture;
                    existing.ProviderSub = item.ProviderSub;
                    existing.ProviderName = item.ProviderName;
                    existing.ModifiedUtc = DateTime.UtcNow;
                }
                else
                {
                    // Add new user
                    _db.Users.Add(item);
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}