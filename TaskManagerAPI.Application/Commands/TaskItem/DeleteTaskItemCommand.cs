using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Application.Commands;

public sealed record DeleteTaskItemCommand
(
    string Id    
) : IRequest<Task>;
