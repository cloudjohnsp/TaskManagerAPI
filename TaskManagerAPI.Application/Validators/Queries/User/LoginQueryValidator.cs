using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;

namespace TaskManagerAPI.Application.Validators;

public sealed class LoginQueryValidator
    : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(command => command.NickName)
            .NotEmpty();
        RuleFor(command => command.Password)
            .NotEmpty();
    }
}
