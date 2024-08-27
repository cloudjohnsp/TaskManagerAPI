using MediatR;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Commands;

public sealed record CreateTaskListCommand(
    string Name,
    string UserId
) : IRequest<TaskList>;

