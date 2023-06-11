using Application.Authentication.Shared;
using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Application.Shared.Interfaces.Utilities;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHashService _passwordHashService;

        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate that the user exists.
            if (_userRepository.GetUserByUsername(request.Username) is not User user)
            {
                throw new BadCredentialsException();
            }

            // 2. Validate that the password is correct
            if (!_passwordHashService.VerifyPassword(request.Password.ToLower(), user.Password))
            {
                throw new BadCredentialsException();
            }

            // 3. Generate JWT Token
            var token = _jwtTokenService.GenerateToken(user);

            // 4. Generate and set Refresh Token
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            _jwtTokenService.SetRefreshToken(refreshToken, user);

            return new AuthenticationResult(user, token);
        }
    }
}
