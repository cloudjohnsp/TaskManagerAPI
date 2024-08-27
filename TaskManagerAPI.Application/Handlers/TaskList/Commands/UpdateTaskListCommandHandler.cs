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

public class UpdateTaskListCommandHandler : IRequestHandler<UpdateTaskListCommand, TaskList?>
{
    private readonly ITaskListRepository _taskListRepository;
    public UpdateTaskListCommandHandler(ITaskListRepository taskListRepository) =>
        _taskListRepository = taskListRepository;

    public async Task<TaskList?> Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
    {
        return await _taskListRepository
            .Update(request.Id, request.Name);
    }
    
}
