using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Exceptions.BaseExceptions;

namespace TaskManagerAPI.Domain.Exceptions;

public sealed class TaskListNotFoundException : NotFoundException
{
    public TaskListNotFoundException(string id) 
        : base($"The TaskList with the Id: {id} was not found")
    { }
}
