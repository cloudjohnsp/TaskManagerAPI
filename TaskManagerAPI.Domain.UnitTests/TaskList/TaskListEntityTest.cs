using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Domain.UnitTests;

public class TaskListEntityTest
{
    [Fact]
    public void Create_Returns_NewTaskListEntity()
    {
        // Arrange
        // Act
        TaskList result = TaskList.Create("Gym Task", "ea44ebc1-d14b-4d81-ad94-78884d91e53f");
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskList>();
        result.Name.Should().Be("Gym Task");
        result.UserId.Should().Be("ea44ebc1-d14b-4d81-ad94-78884d91e53f");
    }
}
