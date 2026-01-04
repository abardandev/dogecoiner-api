using DogeCoiner.Data.DAL.Repos.Users.Strategies;
using DogeCoiner.Data.Dtos;

namespace DogeCoiner.Data.DAL.Repos.Users;

public class UsersRepo : IUsersRepo
{
    private readonly ISaveUsers _saveUsers;
    private readonly CoinDataDbContext _db;

    public UsersRepo(ISaveUsers saveUsers, CoinDataDbContext db)
    {
        _saveUsers = saveUsers;
        _db = db;
    }

    public async Task SaveAsync(User user)
    {
        await _saveUsers.SaveAsync([user]);
    }

    public async Task SaveAsync(User[] items)
    {
        await _saveUsers.SaveAsync(items);
    }

    public bool UserExists(string email)
    {
        return _db.Users.Any(u => u.Email == email);
    }
}