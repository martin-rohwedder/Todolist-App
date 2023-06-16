using MediatR;

namespace Application.UserDetail.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string OldPassword,
        string NewPassword,
        string Username) : IRequest<bool>;
}
