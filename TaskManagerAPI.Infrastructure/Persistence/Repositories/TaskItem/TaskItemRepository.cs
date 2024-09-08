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
    public async Task CreateAsync(TaskItem taskItem)
    {
        await _dbContext
             .TaskItems.AddAsync(taskItem);
        await _dbContext.SaveChangesAsync();
    }

    public void DeleteAsync(TaskItem taskItem)
    {
        _dbContext.TaskItems.Remove(taskItem);
        _dbContext.SaveChanges();
    }

    public async Task<TaskItem?> GetAsync(string id)
        => await _dbContext.TaskItems
            .FindAsync(id);


    public async Task<TaskItem> UpdateAsync(TaskItem taskItem, string description)
    {
        taskItem.Description = description;
        taskItem.CreatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return taskItem;
    }

    public async Task<TaskItem> UpdateStatusAsync(TaskItem taskItem, bool isDone)
    {
        taskItem.IsDone = isDone;
        taskItem.LastUpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return taskItem;
    }
}
