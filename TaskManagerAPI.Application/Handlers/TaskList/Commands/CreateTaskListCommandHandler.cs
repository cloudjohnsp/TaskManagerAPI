using MediatR;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Commands;

public class CreateTaskListCommandHandler : IRequestHandler<CreateTaskListCommand, TaskList>
{
    private readonly ITaskListRepository _tasklistRepository;
    public CreateTaskListCommandHandler(ITaskListRepository tasklistRepository)
    {
        _tasklistRepository = tasklistRepository;
    }

    public async Task<TaskList> Handle(CreateTaskListCommand request, CancellationToken cancellationToken)
    {
        TaskList newTaskList = TaskList.Create(request.Name, request.UserId);
        return await _tasklistRepository.Create(newTaskList);
    }
}
