using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Todolist.Shared;
using Domain.Entities;
using MediatR;

namespace Application.Todolist.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResult>
    {
        private readonly IUserRepository _userRepository;

        public CreateTaskCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<TaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate that the message in the task is not an empty string
            if (string.IsNullOrEmpty(request.Message))
            {
                throw new TodoTaskEmptyMessageException();
            }

            // 2. create a new todo task entity and associate it with a user and persist it
            var user = _userRepository.GetUserByUsername(request.Username);
            var todoTask = new TodoTask
            {
                Message = request.Message,
                User = user,
            };

            // 3. Respond with the Task Result object
            return new TaskResult(todoTask);
        }
    }
}
