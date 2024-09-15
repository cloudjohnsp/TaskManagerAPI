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
using TaskManagerAPI.Contracts;
using System.Security.Authentication;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Auth;
using TaskManagerAPI.Application.Queries;

namespace TaskManagerAPI.Application.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    public LoginQueryHandler(IUserRepository userRepository, IJwtGenerator jwtGenerator)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        User? user = await _userRepository
            .GetByNickNameAsync(query.NickName) ?? throw new CredentialsInvalidException(
                $@"User with nickname: {query.NickName} does not exists"
            );

        if (BC.Verify(query.Password, user.Password) is not true)
        {
            throw new CredentialsInvalidException("Password is incorrect.");
        }

        var token = _jwtGenerator.GenerateToken(user);

        return new LoginResult(
            user,
            token
        );
    }
}
