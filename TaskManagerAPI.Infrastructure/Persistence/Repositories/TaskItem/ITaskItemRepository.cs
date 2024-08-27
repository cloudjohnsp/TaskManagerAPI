using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface ITaskItemRepository
{
    Task<TaskItem> Create(TaskItem item);
    Task<TaskItem?> GetTaskItem(string id);
    Task<TaskItem?> Update(string id, string description, bool isDone);
    Task Delete(string id);
}
