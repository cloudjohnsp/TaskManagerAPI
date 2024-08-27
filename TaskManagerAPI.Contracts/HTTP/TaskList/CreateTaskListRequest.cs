using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record CreateTaskListRequest
(
    string Name,
    string UserId
);
