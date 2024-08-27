using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Exceptions.BaseExceptions;

namespace TaskManagerAPI.Domain.Exceptions;

public sealed class TaskItemNotFoundException : NotFoundException
{
    public TaskItemNotFoundException(string id) 
        : base($"TaskItem with Id: {id} does not exist.") 
    { }
}
