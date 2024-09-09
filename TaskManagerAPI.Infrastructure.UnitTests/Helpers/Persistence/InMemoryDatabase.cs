using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class InMemoryDatabase
{

    public static TaskManagerContext GetInMemoryDbContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        return new TaskManagerContext(options);
    }

    public static void InitializeDatabase(TaskManagerContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
