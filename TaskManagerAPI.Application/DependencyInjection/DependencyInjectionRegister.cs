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
        services.AddMediatR(config =>
        {
            config
            .RegisterServicesFromAssembly(typeof(DependencyInjectionRegister).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}
