using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Commands;

public class DeleteTaskListCommandHandler : IRequestHandler<DeleteTaskListCommand, Task>
{
    private readonly ITaskListRepository _taskListRepository;
    public DeleteTaskListCommandHandler(ITaskListRepository taskListRepository) 
        => _taskListRepository = taskListRepository;

    public async Task<Task> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
    {
        await _taskListRepository.Delete(request.Id);
        return Task.CompletedTask;
    }
}
