using Application.Todolist.Shared;
using MediatR;

namespace Application.Todolist.Commands.UpdateTask
{
    public record UpdateTaskCommand(
        Guid Uuid,
        string Message,
        bool IsCompleted,
        bool IsArchived,
        string Username) : IRequest<TaskResult>;
}
