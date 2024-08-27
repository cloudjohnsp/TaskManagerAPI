using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Queries;

public sealed class GetTaskItemQueryHandler : IRequestHandler<GetTaskItemQuery, TaskItem?>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTaskItemQueryHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItem?> Handle(GetTaskItemQuery request, CancellationToken cancellationToken)
    {
        return await _taskItemRepository.GetTaskItem(request.Id);
    }
}
