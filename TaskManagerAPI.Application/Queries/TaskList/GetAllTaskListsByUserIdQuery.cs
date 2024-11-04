using MediatR;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Queries;

public record GetAllTaskListsByUserIdQuery(
  string UserId
) : IRequest<IEnumerable<TaskList>?>;