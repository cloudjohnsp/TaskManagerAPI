﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Validators;

public sealed class UpdateTaskListCommandValidator 
    : AbstractValidator<UpdateTaskListCommand>
{
    public UpdateTaskListCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.Name)
            .Length(1,50)
            .NotEmpty();
    }
}
