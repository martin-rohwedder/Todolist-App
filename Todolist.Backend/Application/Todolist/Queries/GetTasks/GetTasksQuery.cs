using Domain.Entities;
using MediatR;

namespace Application.Todolist.Queries.GetTasks
{
    public record GetTasksQuery(
        string Username) : IRequest<List<TodoTask>>;
}
