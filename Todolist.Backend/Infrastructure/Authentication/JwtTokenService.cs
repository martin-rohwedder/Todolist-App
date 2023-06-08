using Application.Shared.Interfaces.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private const string RefreshTokenCookieName = "refresh_token";

        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IOptions<JwtSettings> jwtOptions, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.SecretKey!)),
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7)
            };

            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenCookieName, refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenCreated = refreshToken.DateTimeCreated;
            user.RefreshTokenExpires = refreshToken.Expires;
        }

        public string GetRefreshTokenCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenCookieName]!;
        }
    }
}
