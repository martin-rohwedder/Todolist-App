using Application.Shared.Interfaces.Utilities;

namespace Application.Shared.Services.Utilities
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
