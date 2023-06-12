using Application.Authentication.Shared;
using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHashService _passwordHashService;

        public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate if user already exists in the user repository
            if (_userRepository.GetUserByUsername(request.Username) is not null)
            {
                throw new DuplicateUsernameException();
            }

            // 2. Create a new user and persist it in the database
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = _passwordHashService.HashPassword(request.Password.ToLower())
            };

            _userRepository.AddUser(user);

            // 3. Generate JWT Token
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
