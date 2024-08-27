using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers.Commands;

public sealed class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Task>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<Task> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        await _taskItemRepository.Delete(request.Id);
        return Task.CompletedTask;
    }
}
