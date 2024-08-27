using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;

public class TaskManagerContext : DbContext
{
    public TaskManagerContext() : base() { }
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
