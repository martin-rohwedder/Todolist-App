using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using MediatR;

namespace Application.UserDetail.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Check that the new password is not empty
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                throw new ChangePasswordEmptyPasswordException();
            }

            // 2. Check that the user exists
            var user = _userRepository.GetUserByUsername(request.Username);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            // 3. Check that the users current password, match with old password from request
            if (!_passwordHashService.VerifyPassword(request.OldPassword, user.Password))
            {
                throw new ChangePasswordMismatchException();
            }

            // 4. Hash new password and store it in user details, and update (persist) user.
            user.Password = _passwordHashService.HashPassword(request.NewPassword);
            user.DateTimeUpdated = DateTime.UtcNow;

            _userRepository.UpdateUser();

            // 5. Return bool value (true), as response that the password has been changed
            return true;
        }
    }
}
