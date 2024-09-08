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

public sealed class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Task>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<Task> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken)
    {
        TaskItem? taskItem = await _taskItemRepository
            .GetAsync(command.Id) ?? throw new TaskItemNotFoundException(command.Id);
        _taskItemRepository.DeleteAsync(taskItem);
        return Task.CompletedTask;
    }
}
