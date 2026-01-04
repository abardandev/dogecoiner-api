using DogeCoiner.Data.Dtos;

namespace DogeCoiner.Data.DAL.Repos.Users
{
    public interface IUsersRepo
    {
        Task SaveAsync(User user);
        Task SaveAsync(User[] users);
        bool UserExists(string email);
    }
}