using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Exceptions;

public sealed class TaskItemCreateFailureException : InternalErrorException
{
    public TaskItemCreateFailureException() 
        : base("An error happened while attempting to create the taskitem!")
    {
    }
}
