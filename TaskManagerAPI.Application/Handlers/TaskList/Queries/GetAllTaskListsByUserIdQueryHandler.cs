using MediatR;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Queries;

public class GetAllTaskListsByUserIdQueryHandler : IRequestHandler<GetAllTaskListsByUserIdQuery, IEnumerable<TaskList>?>
{
  private readonly ITaskListRepository _taskListRepository;

  public GetAllTaskListsByUserIdQueryHandler(ITaskListRepository taskListRepository)
  {
    _taskListRepository = taskListRepository;
  }

  public async Task<IEnumerable<TaskList>?> Handle(GetAllTaskListsByUserIdQuery request, CancellationToken cancellationToken)
  {
    return await _taskListRepository.GetAllByUserIdAsync(request.UserId);
  }
}
