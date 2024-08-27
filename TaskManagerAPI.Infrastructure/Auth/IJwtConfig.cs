using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Auth;

public interface IJwtConfig
{
    Task<User?> GetUserFromClaims(string token);
    Task<string> GenerateJwtToken(User user);
}
