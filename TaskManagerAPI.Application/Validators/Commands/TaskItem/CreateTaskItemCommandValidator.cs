using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;
public sealed class CreateTaskItemCommandValidator 
    : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator() 
    {
        RuleFor(command => command.Description)
            .NotEmpty()
            .Length(1,50);
        RuleFor(command => command.TaskListId)
            .NotEmpty();
    }
}
