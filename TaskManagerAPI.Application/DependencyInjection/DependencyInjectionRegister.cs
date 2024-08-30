using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using MediatR;
using TaskManagerAPI.Application.Behaviors;

namespace TaskManagerAPI.Application.DependencyInjection;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddMediatR(config =>
        {
            config
            .RegisterServicesFromAssembly(typeof(DependencyInjectionRegister).Assembly)
                .AddOpenBehavior(typeof(ValidationBehaviour<,>))
                .AddOpenBehavior(typeof(LoggingBehaviour<,>));

        })
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
