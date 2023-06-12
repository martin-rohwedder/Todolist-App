using Application.Shared.Interfaces.Persistance;
using Domain.Entities;
using Infrastructure.DataAccess;

namespace Infrastructure.Persistance
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoListDbContext _dbContext;

        public TodoTaskRepository(TodoListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddTodoTask(TodoTask todoTask)
        {
            _dbContext.Add(todoTask);
            _dbContext.SaveChangesAsync();
        }
    }
}
