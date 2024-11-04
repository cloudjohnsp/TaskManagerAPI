using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly TaskManagerContext _dbContext;

    public TaskListRepository(TaskManagerContext context)
    {
        _dbContext = context;
    }

    public async Task CreateAsync(TaskList taskList)
    {
        await _dbContext.TaskLists
            .AddAsync(taskList);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<TaskList?> GetAsync(string id) =>
         await _dbContext.TaskLists
            .Where(task => task.Id == id)
            .Include(taskItem => taskItem.TaskItems)
            .FirstOrDefaultAsync();


    public async Task<IEnumerable<TaskList>?> GetAllAsync() =>
        await _dbContext.TaskLists
            .Include(taskList => taskList.TaskItems)
            .ToListAsync();

    public async Task<TaskList?> UpdateAsync(TaskList taskList, string name)
    {
        taskList.Name = name;
        taskList.LastUpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return taskList;
    }

    public void DeleteAsync(TaskList taskList)
    {
        _dbContext.TaskLists.Remove(taskList);
        _dbContext.SaveChanges();
    }

    public async Task<IEnumerable<TaskList>> GetAllByUserIdAsync(string id)
    {
        return await _dbContext.TaskLists.Where(task => task.UserId == id)
            .Include(taskList => taskList.TaskItems)
            .ToListAsync();
    }
}
