using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public sealed class UpdateTaskItemCommandValidator 
    : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator() 
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.Description)
            .NotEmpty();
    }
}
