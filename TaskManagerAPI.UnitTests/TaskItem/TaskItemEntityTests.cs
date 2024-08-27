using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Domain.UnitTests;

public class TaskItemEntityTests
{
    [Fact]
    public void CreateMethod_Returns_NewTaskItemEntity()
    {
        // Arrange
        // Act
        TaskItem result = TaskItem.Create("Chest Workout", "ea44ebc1-d14b-4d81-ad94-78884d91e53f");
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskItem>();
        result.Description.Should().Be("Chest Workout");
    }
}
