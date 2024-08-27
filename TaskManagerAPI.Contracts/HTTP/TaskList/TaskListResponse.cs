using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record TaskListResponse
(
    string Id,
    string Name,
    DateTime CreatedAt,
    DateTime LastUpdatedAt,
    IEnumerable<TaskItemResponse> TaskItems,
    string UserId
);
