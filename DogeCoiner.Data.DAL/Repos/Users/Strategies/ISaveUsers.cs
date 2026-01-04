using DogeCoiner.Data.Dtos;

namespace DogeCoiner.Data.DAL.Repos.Users.Strategies
{
    public interface ISaveUsers
    {
        Task SaveAsync(User[] users);
    }
}