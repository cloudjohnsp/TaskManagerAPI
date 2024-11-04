using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface ITaskItemRepository
{
    Task CreateAsync(TaskItem taskItem);
    Task<TaskItem?> GetAsync(string id);
    Task<TaskItem> UpdateAsync(TaskItem taskItem, string description);
    Task<TaskItem> UpdateStatusAsync(TaskItem taskItem, bool isDone);
    Task<IEnumerable<TaskItem>?> GetAllByTaskListId(string id);
    void DeleteAsync(TaskItem taskItem);
}
