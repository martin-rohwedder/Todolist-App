using Application.Authentication.Shared;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate if user already exists in the user repository
            if (_userRepository.GetUserByUsername(request.Username) is not null)
            {
                throw new Exception("User already exists");
            }

            // 2. Create a new user and persist it in the database
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password
            };

            _userRepository.AddUser(user);

            //TODO: 3. Generate JWT Token
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
