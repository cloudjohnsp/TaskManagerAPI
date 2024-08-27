using Mapster;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Api.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, CreateUserResponse>();
        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.TaskLists, src => src.TasksLists);
    }
}
