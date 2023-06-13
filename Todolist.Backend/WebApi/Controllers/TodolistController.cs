using Application.Todolist.Commands.CreateTask;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Contracts.Todolist;

namespace WebApi.Controllers
{
    public class TodolistController : ApiControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public TodolistController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost($"{ControllerRoutePath}Task/Create")]
        public async Task<IActionResult> CreateTask(CreateTaskRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<CreateTaskCommand>();
            var taskResult = await _mediator.Send(command);

            return Ok(_mapper.Map<TaskResponse>(taskResult));
        }
    }
}
