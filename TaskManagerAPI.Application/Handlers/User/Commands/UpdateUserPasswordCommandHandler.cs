using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers;

public sealed class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, Task>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserPasswordCommandHandler(IUserRepository userRepository)
    { 
        _userRepository = userRepository;
    }

    public async Task<Task> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdatePassword(request.Id, request.Password);
        return Task.CompletedTask;
    }
}
