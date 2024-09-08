using MediatR;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;
using TaskManagerAPI.Contracts;

namespace TaskManagerAPI.Application.Commands;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User? userExists = await _userRepository
            .GetByNickNameAsync(command.NickName);

        if (userExists is not null)
        {
            throw new UserConflictException(command.NickName);
        }

        User user = User.Create(command.NickName, BC.HashPassword(command.Password), command.Role);
        await _userRepository.CreateAsync(user);
        User? result = await _userRepository.GetAsync(user.Id);

        return result ?? throw new UserNotFoundException(user.Id);
    }
}
