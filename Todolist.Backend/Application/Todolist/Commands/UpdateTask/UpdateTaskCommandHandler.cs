using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Application.Todolist.Shared;
using MediatR;

namespace Application.Todolist.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITodoTaskRepository _todoTaskRepository;

        public UpdateTaskCommandHandler(IUserRepository userRepository, ITodoTaskRepository todoTaskRepository)
        {
            _userRepository = userRepository;
            _todoTaskRepository = todoTaskRepository;
        }

        public async Task<TaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Fetch user object from database and check if user retrieved is null, if so throw exception
            var user = _userRepository.GetUserByUsername(request.Username);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            // 2. Fetch and validate that a task is found
            var task = _todoTaskRepository.GetTaskByUuid(request.Uuid);
            if (task is null)
            {
                throw new TaskNotFoundException();
            }

            // 3. Validate that the updated message is not empty
            if (string.IsNullOrEmpty(request.Message))
            {
                throw new TodoTaskEmptyMessageException();
            }

            // 4. Update the task and persist it to the database again
            task.Message = request.Message;
            task.IsCompleted = request.IsCompleted;
            task.IsArchived = request.IsArchived;
            task.DateTimeUpdated = DateTime.UtcNow;

            _todoTaskRepository.UpdateTasks();

            // 5. Return the updated task result
            return new TaskResult(task);
        }
    }
}
