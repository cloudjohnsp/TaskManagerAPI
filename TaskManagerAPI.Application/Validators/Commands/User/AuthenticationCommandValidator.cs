using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public sealed class AuthenticationCommandValidator 
    : AbstractValidator<AuthenticationCommand>
{
    public AuthenticationCommandValidator()
    {
        RuleFor(command => command.NickName)
            .NotEmpty();
        RuleFor(command => command.Password)
            .NotEmpty();
    }
}
