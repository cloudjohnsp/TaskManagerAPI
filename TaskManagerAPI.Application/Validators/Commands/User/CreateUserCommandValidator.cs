using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public class CreateUserCommandValidator 
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator() 
    {
        RuleFor(command => command.NickName)
            .NotEmpty();
        RuleFor(command => command.Password)
            .NotEmpty()
            .Matches("/^(?=.* [0 - 9])(?=.* [a - z])(?=.* [A - Z])(?=.*\\W)(?!.* ).{8,16}$/")
            .WithMessage("Password must contain at least: 1 uppercase and 1 lowercase letter, 1 special character and must be 8-16 characters long! Ex: Password@1234567");
    } 
}
