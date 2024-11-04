using MediatR;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Queries;


public class GetAllTaskItemsByTaskListIdQueryHandler : IRequestHandler<GetAllTaskItemsByTaskListIdQuery, IEnumerable<TaskItem>?>
{

  private readonly ITaskItemRepository _taskItemRepository;

  public GetAllTaskItemsByTaskListIdQueryHandler(ITaskItemRepository taskItemRepository)
  {
    _taskItemRepository = taskItemRepository;
  }

  public async Task<IEnumerable<TaskItem>?> Handle(GetAllTaskItemsByTaskListIdQuery request, CancellationToken cancellationToken)
  {
    return await _taskItemRepository.GetAllByTaskListId(request.TaskListId);
  }
}