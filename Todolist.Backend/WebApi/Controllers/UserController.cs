using Application.UserDetail.Commands.ChangePassword;
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
        private readonly ILogger<UserController> _logger;

        public UserController(ISender mediator, IMapper mapper, ILogger<UserController> logger)
            : base(mediator, mapper)
        {
            _logger = logger;
        }

        [HttpPut($"{ControllerRoutePath}Update")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Update User Request: Found authenticated user with username {Username}", username);

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<UpdateUserCommand>();
            var userDetailResult = await _mediator.Send(command);

            var userResponse = _mapper.Map<UserDetailResponse>(userDetailResult);
            _logger.LogDebug("Update User Request: {UserResponse}", userResponse);

            return Ok(userResponse);
        }

        [HttpPut($"{ControllerRoutePath}ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Change Password Request: Found authenticated user with username {Username}", username);

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<ChangePasswordCommand>();
            var passwordChangedResult = await _mediator.Send(command);

            _logger.LogDebug("Change Password Request: Password Has Changed Response is {PasswordChangedResult}", passwordChangedResult);

            return Ok(new { PasswordHasChanged = passwordChangedResult });
        }
    }
}
