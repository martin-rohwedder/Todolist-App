using Application.Shared.Interfaces.Utilities;

namespace Application.Shared.Services.Utilities
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return !BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
