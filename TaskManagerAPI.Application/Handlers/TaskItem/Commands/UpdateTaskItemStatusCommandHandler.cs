using MediatR;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Commands;

public class UpdateTaskItemStatusCommandHandler : IRequestHandler<UpdateTaskItemStatusCommand, TaskItem?>
{
  private readonly ITaskItemRepository _taskItemRepository;

  public UpdateTaskItemStatusCommandHandler(ITaskItemRepository taskItemRepository)
  {
    _taskItemRepository = taskItemRepository;
  }
  public async Task<TaskItem?> Handle(UpdateTaskItemStatusCommand request, CancellationToken cancellationToken)
  {
    TaskItem? taskItem = await _taskItemRepository
      .GetAsync(request.Id) ?? throw new TaskItemNotFoundException(request.Id);

    return await _taskItemRepository.UpdateStatusAsync(taskItem, request.IsDone);
  }
}
