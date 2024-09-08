using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Exceptions;

public abstract class InternalErrorException : Exception
{
    protected InternalErrorException(string message) 
        : base(message) {}
}
