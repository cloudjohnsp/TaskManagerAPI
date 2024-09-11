using Mapster;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Api.Mappings;

public sealed class TaskItemMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTaskItemRequest, CreateTaskItemCommand>();
        config.NewConfig<TaskItem, TaskItemResponse>();
    }
}
