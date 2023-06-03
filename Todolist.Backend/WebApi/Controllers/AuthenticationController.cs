using Application.Authentication.Commands.Register;
using Application.Authentication.Queries.Login;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Authentication;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("Api/[controller]/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            var authResult = await _mediator.Send(command);

            return Ok(_mapper.Map<AuthenticationResponse>(authResult));
        }

        [HttpPost("Api/[controller]/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResult = await _mediator.Send(query);

            return Ok(_mapper.Map<AuthenticationResponse>(authResult));
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("This is a test, and can only be accessed by an authroized user");
        }
    }
}
