using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Repositories;

public interface ITaskListRepository
{
    Task<TaskList> Create(TaskList taskList);
    Task<TaskList?> Get(string id);
    Task<IEnumerable<TaskList>?> GetAll();
    Task<TaskList?> Update(string id, string name);
    Task Delete(string id);
}
