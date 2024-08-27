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

public sealed class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskManagerContext _dbContext;
    public TaskItemRepository(TaskManagerContext context)
    {
        _dbContext = context;
    }
    public async Task<TaskItem> Create(TaskItem item)
    {
        EntityEntry<TaskItem> result = await _dbContext
            .TaskItems.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task Delete(string id)
    {
        TaskItem? taskItem = await _dbContext.TaskItems.FindAsync(id);
        if (taskItem != null)
        {
            _dbContext.TaskItems.Remove(taskItem);
            _dbContext.SaveChanges();
            return;
        }

        throw new TaskItemNotFoundException(id);
    }

    public async Task<TaskItem?> GetTaskItem(string id)
    {
        TaskItem? taskItem = await _dbContext.TaskItems
            .FirstOrDefaultAsync(task => task.Id == id);

        return taskItem ?? throw new TaskItemNotFoundException(id);
    }

    public async Task<TaskItem?> Update(string id, string description, bool isDone)
    {
        TaskItem? taskItemToUpdate = await _dbContext.TaskItems
            .FindAsync(id);

        if (taskItemToUpdate != null)
        {
            taskItemToUpdate.Description = description;
            taskItemToUpdate.CreatedAt = DateTime.Now;
            taskItemToUpdate.IsDone = isDone;

            await _dbContext.SaveChangesAsync();
            return taskItemToUpdate;
        }

        throw new TaskItemNotFoundException(id);
    }
}
