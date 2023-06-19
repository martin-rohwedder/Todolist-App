using Application.Todolist.Commands.CreateTask;
using Application.Todolist.Commands.UpdateTask;
using Application.Todolist.Queries.GetTasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Contracts.Todolist;

namespace WebApi.Controllers
{
    public class TodolistController : ApiControllerBase
    {
        private readonly ILogger<TodolistController> _logger;

        public TodolistController(ISender mediator, IMapper mapper, ILogger<TodolistController> logger)
            : base(mediator, mapper)
        {
            _logger = logger;
        }

        [HttpPost($"{ControllerRoutePath}Task/Create")]
        public async Task<IActionResult> CreateTask(CreateTaskRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Create Task Request: Found authenticated user with username {Username}", username);

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<CreateTaskCommand>();
            var taskResult = await _mediator.Send(command);

            var taskResponse = _mapper.Map<TaskResponse>(taskResult);
            _logger.LogDebug("Create Task Request: {TaskResponse}", taskResponse);

            return Ok(taskResponse);
        }

        [HttpGet($"{ControllerRoutePath}Task/GetAll")]
        public async Task<IActionResult> GetTasks()
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Get All Task Request: Found authenticated user with username {Username}", username);

            var query = new GetTasksQuery(username);
            var taskResult = await _mediator.Send(query);

            // Map the task results list to a list of task response objects
            var taskResponseList = taskResult.AsQueryable().ProjectToType<TaskResponse>();
            _logger.LogDebug("Get All Tasks Request: {TaskResponseList}", taskResponseList);

            return Ok(taskResponseList);
        }

        [HttpPut($"{ControllerRoutePath}Task/Update")]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.GivenName)!;

            _logger.LogDebug("Update Task Request: Found authenticated user with username {Username}", username);

            var command = _mapper.From(request).AddParameters("username", username).AdaptToType<UpdateTaskCommand>();
            var taskResult = await _mediator.Send(command);

            var taskResponse = _mapper.Map<TaskResponse>(taskResult);
            _logger.LogDebug("Update Task Request: {TaskResponse}", taskResponse);

            return Ok(taskResponse);
        }
    }
}
