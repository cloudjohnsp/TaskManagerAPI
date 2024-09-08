using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface ITaskListRepository
{
    Task CreateAsync(TaskList taskList);
    Task<TaskList?> GetAsync(string id);
    Task<IEnumerable<TaskList>?> GetAllAsync();
    Task<TaskList?> UpdateAsync(TaskList taskList, string name);
    void DeleteAsync(TaskList taskList);
}
