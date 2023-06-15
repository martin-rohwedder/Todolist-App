using Application.Todolist.Commands.CreateTask;
using Application.Todolist.Commands.UpdateTask;
using Application.Todolist.Shared;
using Mapster;
using WebApi.Contracts.Todolist;

namespace WebApi.Shared.Mapping
{
    public class TodolistMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TaskResult, TaskResponse>()
                .Map(dest => dest, src => src.TodoTask);

            config.NewConfig<CreateTaskRequest, CreateTaskCommand>()
                .Map(dest => dest.Username, src => MapContext.Current!.Parameters["username"]);

            config.NewConfig<UpdateTaskRequest, UpdateTaskCommand>()
                .Map(dest => dest.Username, src => MapContext.Current!.Parameters["username"]);
        }
    }
}
