using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.UserDetail.Shared;
using MediatR;

namespace Application.UserDetail.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Check if user is found in database
            var user = _userRepository.GetUserByUsername(request.Username);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            // 2. Check if request data is not empty
            if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
            {
                throw new UserDetailsEmptyException();
            }

            // 3. Update and persist updated user
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DateTimeUpdated = DateTime.UtcNow;

            _userRepository.UpdateUser();

            // 4. Return updated user results
            return new UserResult(user);
        }
    }
}
