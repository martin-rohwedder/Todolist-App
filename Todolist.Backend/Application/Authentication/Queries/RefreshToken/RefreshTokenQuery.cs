using MediatR;

namespace Application.Authentication.Queries.RefreshToken
{
    public record RefreshTokenQuery(
        string Username) : IRequest<string>;
}
