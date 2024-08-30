using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.Infrastructure.Auth;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.DependencyInjection;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services.AddPersistence(configuration)
            .AddRepositories()
            .AddAuth(configuration);
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        return services
            .AddDbContext<TaskManagerContext>(options => options.UseSqlServer(connectionString));
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ITaskListRepository, TaskListRepository>()
             .AddScoped<ITaskItemRepository, TaskItemRepository>()
             .AddScoped<IUserRepository, UserRepository>();
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"))
            .AddScoped<IJwtConfig, JwtConfig>();
    }
}
