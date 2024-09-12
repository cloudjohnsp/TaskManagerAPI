using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Configurations;

public class TaskListConfiguration : IEntityTypeConfiguration<TaskList>
{
    public void Configure(EntityTypeBuilder<TaskList> builder)
    {
        builder.ToTable("tasklists");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(50);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastUpdatedAt);

        builder.HasMany(x => x.TaskItems)
            .WithOne(x => x.TaskList)
            .HasForeignKey(x => x.TaskListId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.TasksLists)
            .HasForeignKey(x => x.UserId);
    }
}
