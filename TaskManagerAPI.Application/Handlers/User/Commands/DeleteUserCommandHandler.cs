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

namespace TaskManagerAPI.Application.Handlers;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Task>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Task> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userRepository
            .GetAsync(command.Id) ?? throw new UserNotFoundException(command.Id);
        _userRepository.DeleteAsync(user!);
        return Task.CompletedTask;
    }
}
