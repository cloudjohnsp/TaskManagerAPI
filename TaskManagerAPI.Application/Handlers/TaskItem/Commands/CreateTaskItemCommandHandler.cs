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

public sealed class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, TaskItem>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ITaskListRepository _taskListRepository;
    public CreateTaskItemCommandHandler(ITaskItemRepository taskItemRepository, ITaskListRepository taskListRepository)
    {
        _taskItemRepository = taskItemRepository;
        _taskListRepository = taskListRepository;
    }

    public async Task<TaskItem> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken)
    {
        if (await _taskListRepository.GetAsync(command.TaskListId) is null) throw new TaskListNotFoundException(command.TaskListId);
        TaskItem newTaskItem = TaskItem.Create(command.Description, command.TaskListId);
        await _taskItemRepository.CreateAsync(newTaskItem);
        TaskItem? result = await _taskItemRepository.GetAsync(newTaskItem.Id);

        return result ?? throw new TaskItemCreateFailureException();
    }
}
