using MediatR;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Commands;

public class CreateTaskListCommandHandler : IRequestHandler<CreateTaskListCommand, TaskList>
{
    private readonly ITaskListRepository _tasklistRepository;
    private readonly IUserRepository _userRepository;
    public CreateTaskListCommandHandler(ITaskListRepository tasklistRepository, IUserRepository userRepository)
    {
        _tasklistRepository = tasklistRepository;
        _userRepository = userRepository;
    }

    public async Task<TaskList> Handle(CreateTaskListCommand command, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetAsync(command.UserId) is null)
        {
            throw new UserNotFoundException(command.UserId);
        }

        TaskList newTaskList = TaskList.Create(command.Name, command.UserId);
        await _tasklistRepository.CreateAsync(newTaskList);
        TaskList? result = await _tasklistRepository
            .GetAsync(newTaskList.Id);
        return result!;
    }
}
