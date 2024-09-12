using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Entities;

public class TaskList : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public List<TaskItem> TaskItems { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
    [JsonIgnore]
    public User User { get; set; } = null!;

    public TaskList() { }

    public TaskList(string id, string name, DateTime createdAt, DateTime lastUpdatedAt, List<TaskItem> taskItems, string userId)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        LastUpdatedAt = lastUpdatedAt;
        TaskItems = taskItems;
        UserId = userId;
    }

    public static TaskList Create(string name, string userId)
        => new(Guid.NewGuid().ToString(), name, DateTime.Now, DateTime.Now, [], userId);
    
}
