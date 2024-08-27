using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Commands;

public sealed record UpdateTaskListCommand
(
    string Id,
    string Name
) : IRequest<TaskList?>;
