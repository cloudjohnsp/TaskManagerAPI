using MediatR;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Queries;

public record GetAllTaskListsQuery
(
     
) : IRequest<IEnumerable<TaskList>?>;
