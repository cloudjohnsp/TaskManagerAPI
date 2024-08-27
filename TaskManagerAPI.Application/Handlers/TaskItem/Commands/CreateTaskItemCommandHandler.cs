using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Commands;

public sealed class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, TaskItem>
{
    private readonly ITaskItemRepository _taskItemRepository;
    public CreateTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItem> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        TaskItem newTaskItem = TaskItem.Create(request.Description, request.TaskListId);
        TaskItem result = await _taskItemRepository.Create(newTaskItem);
        return result;
    }
}
