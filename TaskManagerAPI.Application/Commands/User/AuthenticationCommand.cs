using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Commands;

public sealed record AuthenticationCommand
(
    string NickName,
    string Password
) : IRequest<User?>;