using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface IUserRepository
{
    void CreateAsync(User user);
    Task<User?> GetAsync(string id);
    Task<User?> GetByNickNameAsync(string nickName);
    Task<User> UpdateNickNameAsync(User user, string nickName);
    void UpdatePasswordAsync(User user, string password);
    void DeleteAsync(User user);
}
