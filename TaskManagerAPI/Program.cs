using TaskManagerAPI.Infrastructure.DependencyInjection;
using TaskManagerAPI.Application.DependencyInjection;
using TaskManagerAPI.Api.DependencyInjection;
using TaskManagerAPI.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddApi(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(builder.Configuration.GetValue<string>("CorsSettings:DefaultPolicy")!);

app.MapControllers();

app.Run();
