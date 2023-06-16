using Application.UserDetail.Shared;
using MediatR;

namespace Application.UserDetail.Commands.UpdateUser
{
    public record UpdateUserCommand(
        string FirstName,
        string LastName,
        string Username) : IRequest<UserResult>;
}
