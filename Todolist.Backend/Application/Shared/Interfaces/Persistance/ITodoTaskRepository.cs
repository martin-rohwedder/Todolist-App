using Domain.Entities;

namespace Application.Shared.Interfaces.Persistance
{
    public interface ITodoTaskRepository
    {
        void AddTodoTask(TodoTask todoTask);
    }
}
