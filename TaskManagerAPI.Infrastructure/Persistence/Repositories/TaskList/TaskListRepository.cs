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

    public async Task<TaskList> Create(TaskList taskList)
    {
        var result = await _dbContext.TaskLists
            .AddAsync(taskList);

        await _dbContext
            .SaveChangesAsync();
        return result.Entity;
    }

    public async Task<TaskList?> Get(string id)
    {
        TaskList? taskList = await _dbContext.TaskLists
            .Where(task => task.Id == id)
            .Include(taskItem => taskItem.TaskItems)
            .FirstOrDefaultAsync();

        return taskList ?? throw new TaskListNotFoundException(id);
    }

    public async Task<IEnumerable<TaskList>?> GetAll() =>
        await _dbContext.TaskLists
            .Include(taskList => taskList.TaskItems)
            .ToListAsync();

    public async Task<TaskList?> Update(string id, string name)
    {
        TaskList? taskList = await _dbContext.TaskLists.FindAsync(id);

        if (taskList != null)
        {
            taskList.Name = name;
            taskList.LastUpdatedAt = DateTime.Now;
            _dbContext.Entry(taskList).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return taskList;
        }

        throw new TaskListNotFoundException(id);
    }

    public async Task Delete(string id)
    {

        TaskList? taskList = await _dbContext.TaskLists
            .FindAsync(id);

        if (taskList != null)
        {
            _dbContext.TaskLists.Remove(taskList);
            _dbContext.SaveChanges();
            return;
        }

        throw new TaskListNotFoundException(id);
    }
}
