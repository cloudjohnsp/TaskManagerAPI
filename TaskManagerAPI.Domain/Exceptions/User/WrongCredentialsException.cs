using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Exceptions.BaseExceptions;

namespace TaskManagerAPI.Domain.Exceptions;

public class WrongCredentialsException : NotFoundException
{
    public WrongCredentialsException(string message) : base(message) { }
}
