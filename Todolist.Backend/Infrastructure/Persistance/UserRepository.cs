using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoListDbContext _dbContext;

        public UserRepository(TodoListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChangesAsync();
        }

        public User? GetUserByUsername(string username)
        {
            return _dbContext.Users.SingleOrDefault(user => user.Username == username);
        }

        public void UpdateUser()
        {
            _dbContext.SaveChanges();
        }
    }
}
