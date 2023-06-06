using Application.Shared.Interfaces.Persistance;
using Domain.Entities;

namespace Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new List<User>();

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User? GetUserByUsername(string username)
        {
            return _users.SingleOrDefault(user => user.Username == username);
        }

        public int GetUserCount()
        {
            return _users.Count;
        }
    }
}
