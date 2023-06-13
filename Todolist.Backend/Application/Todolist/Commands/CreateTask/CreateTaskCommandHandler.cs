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
        private readonly ITodoTaskRepository _todoTaskRepository;

        public CreateTaskCommandHandler(IUserRepository userRepository, ITodoTaskRepository todoTaskRepository)
        {
            _userRepository = userRepository;
            _todoTaskRepository = todoTaskRepository;
        }

        public async Task<TaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate that the message in the task is not an empty string
            if (string.IsNullOrEmpty(request.Message))
            {
                throw new TodoTaskEmptyMessageException();
            }

            // 2. Fetch user object from database and check if user retrieved is null
            var user = _userRepository.GetUserByUsername(request.Username);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            // 3. create a new todo task entity and associate it with a user and persist it
            var todoTask = new TodoTask
            {
                Message = request.Message,
                User = user,
            };

            _todoTaskRepository.AddTodoTask(todoTask);

            // 4. Respond with the Task Result object
            return new TaskResult(todoTask);
        }
    }
}
