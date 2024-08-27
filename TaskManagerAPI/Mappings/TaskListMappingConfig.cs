using Mapster;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Api.Mappings;

public sealed class TaskListMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTaskListRequest, CreateTaskListCommand>();
        config.NewConfig<UpdateTaskListRequest, UpdateTaskListCommand>();
        config.NewConfig<TaskList, TaskListResponse>()
            .Map(dest => dest.TaskItems, src => src.TaskItems);

    }
}
