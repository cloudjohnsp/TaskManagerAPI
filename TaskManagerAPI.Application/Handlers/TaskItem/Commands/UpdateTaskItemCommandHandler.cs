using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Commands;

public sealed class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, TaskItem?>
{
    private readonly ITaskItemRepository _taskItemRepository;
    public UpdateTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItem?> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken)
    {
        TaskItem? taskItem = await _taskItemRepository
            .GetAsync(command.Id) ?? throw new TaskItemNotFoundException(command.Id);
        return await _taskItemRepository
            .UpdateAsync(taskItem, command.Description);
    }
}
