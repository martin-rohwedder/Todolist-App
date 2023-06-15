namespace WebApi.Contracts.Todolist
{
    public record UpdateTaskRequest(
        Guid Uuid,
        string Message,
        bool IsCompleted,
        bool IsArchived);
}
