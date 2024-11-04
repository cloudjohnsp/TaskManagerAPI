using MediatR;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Queries;

public record GetAllTaskItemsByTaskListIdQuery(
  string TaskListId
) : IRequest<IEnumerable<TaskItem>?>;