using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.Handlers;

public sealed class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, User?>
{
    private readonly IUserRepository _userRepository;
    public AuthenticationCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository
            .GetByUserCredentials(request.NickName, request.Password);
    }
}
