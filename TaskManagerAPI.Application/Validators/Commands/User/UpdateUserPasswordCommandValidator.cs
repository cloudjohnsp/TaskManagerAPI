﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public sealed class UpdateUserPasswordCommandValidator
    : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.Password)
            .NotEmpty()
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,16}$")
            .WithMessage("Password must contain at least: 1 uppercase and 1 lowercase letter, 1 special character and must be 8-16 characters long! Ex: Password@1234567");
    }
}
