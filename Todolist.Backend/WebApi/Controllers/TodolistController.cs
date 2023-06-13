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

            var taskResult = await _mediator.Send(new CreateTaskCommand(request.Message, username));

            return Ok(_mapper.Map<TaskResponse>(taskResult));
        }
    }
}
