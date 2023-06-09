﻿using Application.Shared.Interfaces.Persistance;
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

        public List<TodoTask> GetAllTasksFromUser(string username)
        {
            return _dbContext.TodoTasks.Where(task => task.User!.Username == username).ToList();
        }

        public TodoTask GetTaskByUuid(Guid uuid)
        {
            return _dbContext.TodoTasks.SingleOrDefault(task => task.Uuid.Equals(uuid))!;
        }

        public void UpdateTasks()
        {
            _dbContext.SaveChanges();
        }
    }
}
