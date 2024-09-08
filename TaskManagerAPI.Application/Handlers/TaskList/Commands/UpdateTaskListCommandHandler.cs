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

public class UpdateTaskListCommandHandler : IRequestHandler<UpdateTaskListCommand, TaskList?>
{
    private readonly ITaskListRepository _taskListRepository;
    public UpdateTaskListCommandHandler(ITaskListRepository taskListRepository) =>
        _taskListRepository = taskListRepository;

    public async Task<TaskList?> Handle(UpdateTaskListCommand command, CancellationToken cancellationToken)
    {
        TaskList? taskList = await _taskListRepository
            .GetAsync(command.Id) ?? throw new TaskListNotFoundException(command.Id);
        return await _taskListRepository
            .UpdateAsync(taskList, command.Name);
    }
    
}
