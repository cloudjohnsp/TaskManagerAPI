using TaskManagerAPI.Infrastructure.DependencyInjection;
using TaskManagerAPI.Application.DependencyInjection;
using TaskManagerAPI.Api.DependencyInjection;
using TaskManagerAPI.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApi();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
