using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using TaskManagerAPI.Api.Configs;
using TaskManagerAPI.Api.Middlewares;

namespace TaskManagerAPI.Api.DependencyInjection;

public static class DependencyInjectionRegister
{

    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddControllers()
            .AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Task Manager API",
                Description = "Web API for managing tasks."
            });

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Type Bearer [space] followed by the JWT Token in the input bellow"
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });


        services.AddMappings();

        services.AddCorsPolicies(configuration);

        return services;
    }

    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        return services.AddSingleton(config)
             .AddScoped<IMapper, ServiceMapper>();

    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, ConfigurationManager configuration)
    {
        CorsSettings corsSettings = new();

        configuration.Bind(CorsSettings.SectionName, corsSettings);

        services.AddCors(options =>
        {
            options.AddPolicy(name: corsSettings.DefaultPolicy,
            policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        return services;
    }
}
