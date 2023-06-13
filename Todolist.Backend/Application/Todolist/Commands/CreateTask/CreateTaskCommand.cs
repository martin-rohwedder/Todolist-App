using Application.Todolist.Shared;
using MediatR;

namespace Application.Todolist.Commands.CreateTask
{
    public record CreateTaskCommand(
        string Message,
        string Username) : IRequest<TaskResult>;
}
