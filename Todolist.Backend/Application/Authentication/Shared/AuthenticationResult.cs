using Domain.Entities;

namespace Application.Authentication.Shared
{
    public record AuthenticationResult(
        User User,
        string Token);
}
