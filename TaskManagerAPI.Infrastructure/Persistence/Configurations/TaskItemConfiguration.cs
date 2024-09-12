using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("taskitems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Description)
            .HasMaxLength(50);

        builder.Property(x => x.IsDone)
            .HasColumnType("BIT");

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastUpdatedAt);

        builder.HasOne(x => x.TaskList)
            .WithMany(x => x.TaskItems)
            .HasForeignKey(x => x.TaskListId);
    }
}
