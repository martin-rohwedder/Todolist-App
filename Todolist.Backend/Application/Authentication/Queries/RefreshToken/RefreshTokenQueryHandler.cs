using Application.Shared.Errors;
using Application.Shared.Interfaces.Authentication;
using Application.Shared.Interfaces.Persistance;
using MediatR;

namespace Application.Authentication.Queries.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenQueryHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var refreshToken = _jwtTokenService.GetRefreshTokenCookie();
            var user = _userRepository.GetUserByUsername(request.Username);

            if (!user!.RefreshToken.Equals(refreshToken))
            {
                throw new RefreshTokenInvalidException();
            }
            else if (user!.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new RefreshTokenExpiredException();
            }
            else
            {
                var token = _jwtTokenService.GenerateToken(user);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
                _jwtTokenService.SetRefreshToken(newRefreshToken, user);

                _userRepository.UpdateUser();

                return token;
            }
        }
    }
}
