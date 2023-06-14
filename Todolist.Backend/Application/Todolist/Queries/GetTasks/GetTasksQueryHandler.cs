using Application.Shared.Errors;
using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using MediatR;

namespace Application.Todolist.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TodoTask>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITodoTaskRepository _todoTaskRepository;

        public GetTasksQueryHandler(IUserRepository userRepository, ITodoTaskRepository todoTaskRepository)
        {
            _userRepository = userRepository;
            _todoTaskRepository = todoTaskRepository;
        }

        public async Task<List<TodoTask>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1. Validate that the user is found
            if (_userRepository.GetUserByUsername(request.Username) is null)
            {
                throw new UserNotFoundException();
            }

            // 2. Fetch all TodoTasks from the database associated with the user and return it as a collection
            return _todoTaskRepository.GetAllTasksFromUser(request.Username);
        }
    }
}
