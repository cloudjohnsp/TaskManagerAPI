﻿using MediatR;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Commands;

public class CreateTaskListCommandHandler : IRequestHandler<CreateTaskListCommand, TaskList>
{
    private readonly ITaskListRepository _tasklistRepository;
    public CreateTaskListCommandHandler(ITaskListRepository tasklistRepository)
    {
        _tasklistRepository = tasklistRepository;
    }

    public async Task<TaskList> Handle(CreateTaskListCommand command, CancellationToken cancellationToken)
    {
        TaskList newTaskList = TaskList.Create(command.Name, command.UserId);
        await _tasklistRepository.CreateAsync(newTaskList);
        TaskList? result = await _tasklistRepository
            .GetAsync(newTaskList.Id);
        return result!;
    }
}
