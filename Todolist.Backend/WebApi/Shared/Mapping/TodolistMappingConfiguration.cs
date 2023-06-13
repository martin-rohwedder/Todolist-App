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
        }
    }
}
