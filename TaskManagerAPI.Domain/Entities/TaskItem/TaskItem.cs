using System.Text.Json.Serialization;

namespace TaskManagerAPI.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string TaskListId { get; set; } = string.Empty;
    [JsonIgnore]
    public TaskList TaskList { get; set; } = null!;

    public TaskItem() { }

    public TaskItem(string id, string description, bool isDone, DateTime createdAt, DateTime lastUpdatedAt, string taskListId) 
    {
        Id = id;
        Description = description;
        IsDone = isDone;
        CreatedAt = createdAt;
        LastUpdatedAt = lastUpdatedAt;
        TaskListId = taskListId;
    }

    public static TaskItem Create(string description, string taskListId)
        => new(Guid.NewGuid().ToString(), description, false, DateTime.Now, DateTime.Now, taskListId);

}
