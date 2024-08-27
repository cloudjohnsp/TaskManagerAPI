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

    public async Task<User?> Handle(UpdateUserNickNameCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.UpdateNickName(request.Id, request.NickName);
    }
}
