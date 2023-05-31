using Application.Authentication.Commands.Register;
using Application.Authentication.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Authentication;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public AuthenticationController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Api/[controller]/Register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(
                request.FirstName, 
                request.LastName, 
                request.Username, 
                request.Password);

            var authResult = await _mediator.Send(command);

            return Ok(authResult);
        }

        [HttpPost("Api/[controller]/Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(
                request.Username,
                request.Password);

            var authResult = await _mediator.Send(query);

            return Ok(authResult);
        }
    }
}
