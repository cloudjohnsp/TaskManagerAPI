using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Entities;

public class TaskList
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public ICollection<TaskItem> TaskItems { get; set; } = [];
    public string UserId { get; set; } = null!;
    [JsonIgnore]
    public User User { get; set; } = null!;

    public TaskList() { }

    protected TaskList(string id, string name, DateTime createdAt, DateTime lastUpdatedAt, List<TaskItem> taskItems, string userId)
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
