using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.NickName);

        builder.Property(x => x.Password);

        builder.Property(x => x.Role);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastUpdatedAt);

        builder.HasMany(x => x.TasksLists)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}
