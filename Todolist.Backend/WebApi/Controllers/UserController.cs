using Application.UserDetail.Commands.UpdateUser;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Contracts.UserDetail;

namespace WebApi.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UserController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut($"{ControllerRoutePath}Update")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<UpdateUserCommand>();
            var userDetailResult = await _mediator.Send(command);

            return Ok(_mapper.Map<UserDetailResponse>(userDetailResult));
        }
    }
}
