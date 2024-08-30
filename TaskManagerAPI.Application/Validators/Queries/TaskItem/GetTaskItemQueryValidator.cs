using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Queries;

namespace TaskManagerAPI.Application.Validators;

public sealed class GetTaskItemQueryValidator 
    : AbstractValidator<GetTaskItemQuery>
{
    public GetTaskItemQueryValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
