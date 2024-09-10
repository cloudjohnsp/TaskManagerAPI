using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers;

public sealed class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, Task>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Task> Handle(UpdateUserPasswordCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userRepository
            .GetAsync(command.Id) ?? throw new UserNotFoundException(command.Id);
        _userRepository.UpdatePasswordAsync(user, BC.HashPassword(command.Password));
        return Task.CompletedTask;
    }
}
