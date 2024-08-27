using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Exceptions.BaseExceptions;

namespace TaskManagerAPI.Domain.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string id) : base($"User with id: {id} not found!") { }
}
