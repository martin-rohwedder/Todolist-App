namespace WebApi.Contracts.Todolist
{
    public record TaskResponse(
        Guid Uuid,
        string Message,
        bool IsCompleted,
        bool IsArchived,
        DateTime DateTimeCreated,
        DateTime DateTimeUpdated);
}
