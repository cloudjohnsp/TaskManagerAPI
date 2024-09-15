using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Contracts;

namespace TaskManagerAPI.Application.Queries;

public record LoginQuery
(
    string NickName,
    string Password
) : IRequest<LoginResult>;
