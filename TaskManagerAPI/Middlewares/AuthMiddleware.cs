using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TaskManagerAPI.Infrastructure.Auth;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Api.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IJwtConfig jwtConfig)
        {
            var token = httpContext.Request.Headers.Authorization
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            if (token != null)
            {
                httpContext.Items["User"] = await jwtConfig.GetUserFromClaims(token);
            }
            await _next(httpContext);
        }
    }   
}
