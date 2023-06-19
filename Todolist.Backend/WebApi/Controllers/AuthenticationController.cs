using Application.Authentication.Commands.Register;
using Application.Authentication.Queries.Login;
using Application.Authentication.Queries.RefreshToken;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Contracts.Authentication;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ISender mediator, IMapper mapper, ILogger<AuthenticationController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;

            _logger = logger;
        }

        [HttpPost($"{ControllerRoutePath}Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            var authResult = await _mediator.Send(command);

            var authResponse = _mapper.Map<AuthenticationResponse>(authResult);
            _logger.LogDebug("Register response: {AuthenticationResponse}", authResponse);

            return Ok(authResponse);
        }

        [HttpPost($"{ControllerRoutePath}Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResult = await _mediator.Send(query);

            var authResponse = _mapper.Map<AuthenticationResponse>(authResult);
            _logger.LogDebug("Login response: {AuthenticationResponse}", authResponse);

            return Ok(authResponse);
        }

        [HttpPost($"{ControllerRoutePath}RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Refresh Token Request: Found authenticated user with username {Username}", username);

            var result = await _mediator.Send(new RefreshTokenQuery(username));

            return Ok(new { Token = result });
        }
    }
}
