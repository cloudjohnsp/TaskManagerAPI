using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagerContext _dbContext;

    public UserRepository(TaskManagerContext dbContext)
        => _dbContext = dbContext;

    public async void CreateAsync(User user)
    {
        await _dbContext.Users
            .AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public void DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public async Task<User?> GetAsync(string id)
    {
        return await _dbContext.Users
            .Where(u => u.Id == id)
            .Include(t => t.TasksLists)
            .ThenInclude(x => x.TaskItems)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByNickNameAsync(string nickName)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(n => n.NickName == nickName);
    }

    public async Task<User> UpdateNickNameAsync(User user, string nickName)
    {
        user.NickName = nickName;
        user.LastUpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async void UpdatePasswordAsync(User user, string password)
    {
        user.Password = password;
        user.LastUpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
    }
}
