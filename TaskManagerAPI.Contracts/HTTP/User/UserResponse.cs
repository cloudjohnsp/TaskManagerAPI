using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record UserResponse
(
    string Id,
    string NickName,
    string Role,
    DateTime CreatedAt,
    DateTime LastUpdatedAt,
    List<TaskListResponse> TaskLists
);
