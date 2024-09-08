using Mapster;
using TaskManagerAPI.Contracts;
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
        config.NewConfig<LoginResult, LoginResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.Role, src => src.User.Role)
            .Map(dest => dest, src => src.User);
    }
}
