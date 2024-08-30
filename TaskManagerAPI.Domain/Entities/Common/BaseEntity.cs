using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Entities;

public class BaseEntity
{
    public string Id { get; set; } = string.Empty;
}
