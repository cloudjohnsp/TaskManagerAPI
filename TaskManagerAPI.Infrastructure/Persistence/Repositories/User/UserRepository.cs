using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
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

    public async Task<User> Create(User user)
    {
        user.Password = BC.HashPassword(user.Password);
        EntityEntry<User> result = await _dbContext.Users
            .AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task Delete(string id)
    {
        User? userToBeDeleted = await _dbContext.Users.FindAsync(id);
        if (userToBeDeleted != null)
        {
            _dbContext.Users.Remove(userToBeDeleted);
            _dbContext.SaveChanges();
            return;
        }

        throw new UserNotFoundException(id);
    }

    public async Task<User?> Get(string id)
    {
        User? user = await _dbContext.Users
            .Where(u => u.Id == id)
            .Include(t => t.TasksLists)
            .ThenInclude(x => x.TaskItems)
            .FirstOrDefaultAsync();
        return user ?? throw new UserNotFoundException(id);
    }

    public async Task<User?> GetByUserCredentials(string nickName, string password)
    {
        User? user = await _dbContext.Users
            .FirstOrDefaultAsync(n => n.NickName == nickName);

        if (user is null || !BC.Verify(password, user!.Password))
        {
            throw new WrongCredentialsException($"User with nickname: {nickName} does not exist or password is incorrect.");
        }

        return user;
    }

    public async Task<User?> UpdateNickName(string id, string nickName)
    {
        User? userToBeUpdated = await _dbContext.Users
            .FindAsync(id);

        if (userToBeUpdated != null)
        {
            userToBeUpdated.NickName = nickName;
            userToBeUpdated.LastUpdatedAt = DateTime.Now;
            _dbContext.Entry(userToBeUpdated).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return userToBeUpdated;
        }

        throw new UserNotFoundException(id);
    }

    public async Task UpdatePassword(string id, string password)
    {
        User? userToBeUpdated = await _dbContext.Users.FindAsync(id);
        if (userToBeUpdated != null)
        {
            userToBeUpdated.Password = BC.HashPassword(password);
            userToBeUpdated.LastUpdatedAt = DateTime.Now;
            _dbContext.Entry(userToBeUpdated).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return;
        }
        throw new UserNotFoundException(id);
    }
}
