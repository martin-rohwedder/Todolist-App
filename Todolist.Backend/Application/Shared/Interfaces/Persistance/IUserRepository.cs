using Domain.Entities;

namespace Application.Shared.Interfaces.Persistance
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetUserByUsername(string username);
        int GetUserCount();
    }
}
