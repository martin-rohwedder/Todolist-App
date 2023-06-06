using Domain.Entities;

namespace Application.Shared.Interfaces.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        RefreshToken GenerateRefreshToken();
        void SetRefreshToken(RefreshToken refreshToken, User user);
    }
}
