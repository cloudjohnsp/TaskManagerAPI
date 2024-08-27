using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public sealed class CreateTaskListCommandValidator
    : AbstractValidator<CreateTaskListCommand>
{
    public CreateTaskListCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .Length(1, 50);

        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
