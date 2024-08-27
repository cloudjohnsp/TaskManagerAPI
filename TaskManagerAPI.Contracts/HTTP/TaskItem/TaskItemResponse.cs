using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Contracts.HTTP;

public sealed record TaskItemResponse
(
    string Id,
    string Description,
    bool IsDone,
    DateTime CreatedAt,
    DateTime LastUpdatedAt,
    string TaskListId
);
