using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;

namespace TaskManagerAPI.Application.Validators;

public sealed class UpdateUserNickNameCommandValidator 
    : AbstractValidator<UpdateUserNickNameCommand>
{
    public UpdateUserNickNameCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.NickName)
            .NotEmpty();
    }
}
