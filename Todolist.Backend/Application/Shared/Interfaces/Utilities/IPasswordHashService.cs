namespace Application.Shared.Interfaces.Utilities
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
    }
}
