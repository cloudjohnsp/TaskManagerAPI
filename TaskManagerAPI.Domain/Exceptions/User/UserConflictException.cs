using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Exceptions;

public class UserConflictException : ConflictException
{
    public UserConflictException(string nickName) : base($"Nickname: {nickName} is already in use!")
    {
    }
}
