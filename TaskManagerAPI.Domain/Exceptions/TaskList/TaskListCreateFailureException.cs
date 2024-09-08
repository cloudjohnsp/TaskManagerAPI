using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Exceptions;

public sealed class TaskListCreateFailureException : InternalErrorException
{
    public TaskListCreateFailureException() 
        : base("An error happened while attempting to create the tasklist!")
    {
    }
}
