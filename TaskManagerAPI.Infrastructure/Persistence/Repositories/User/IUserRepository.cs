using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface IUserRepository
{
    Task<User> Create(User user);
    Task<User?> Get(string id);
    Task<User?> GetByUserCredentials(string nickName, string password);
    Task<User?> UpdateNickName(string id, string nickName);
    Task UpdatePassword(string id, string password);
    Task Delete(string id);
}
