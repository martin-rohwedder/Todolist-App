using Application.Authentication.Shared;
using MediatR;

namespace Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Username,
        string Password) : IRequest<AuthenticationResult>;
}
