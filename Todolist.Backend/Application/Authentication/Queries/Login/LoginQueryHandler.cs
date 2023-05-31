using Application.Authentication.Shared;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate that the user exists.
            if (_userRepository.GetUserByUsername(request.Username) is not User user)
            {
                throw new Exception("User is not found");
            }

            // 2. Validate that the password is correct
            if (user.Password != request.Password) 
            {
                throw new Exception("Bad Credentials");
            }

            // 3. Generate JWT Token
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
