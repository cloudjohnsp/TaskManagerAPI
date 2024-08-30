using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Queries;

namespace TaskManagerAPI.Application.Validators;

public sealed class GetUserQueryValidator 
    : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}
