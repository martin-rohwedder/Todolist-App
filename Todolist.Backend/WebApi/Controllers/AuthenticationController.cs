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
        private const string ControllerRoutePath = "Api/[controller]/";

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost($"{ControllerRoutePath}Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            var authResult = await _mediator.Send(command);

            return Ok(_mapper.Map<AuthenticationResponse>(authResult));
        }

        [HttpPost($"{ControllerRoutePath}Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResult = await _mediator.Send(query);

            return Ok(_mapper.Map<AuthenticationResponse>(authResult));
        }

        [HttpPost($"{ControllerRoutePath}RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            var result = await _mediator.Send(new RefreshTokenQuery(username));

            return Ok(new { Token = result });
        }
    }
}
