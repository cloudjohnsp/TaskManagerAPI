using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;
using TaskManagerAPI.Domain.Exceptions;

namespace TaskManagerAPI.Application.Handlers;

public sealed class UpdateUserNickNameCommandHandler : IRequestHandler<UpdateUserNickNameCommand, User?>
{

    private readonly IUserRepository _userRepository;

    public UpdateUserNickNameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(UpdateUserNickNameCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetAsync(command.Id);
        return user == null
            ? throw new UserNotFoundException(command.Id)
            : await _userRepository
            .UpdateNickNameAsync(user, command.NickName);
    }
}
