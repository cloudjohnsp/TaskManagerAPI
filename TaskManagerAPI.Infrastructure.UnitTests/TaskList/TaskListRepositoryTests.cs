using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class TaskListRepositoryTests
{
    private readonly Mock<TaskManagerContext> _dbContextMock;
    private readonly Mock<DbSet<TaskList>> _taskListDbSetMock;
    private readonly TaskListRepository _mockRepository;

    public TaskListRepositoryTests()
    {
        _dbContextMock = new Mock<TaskManagerContext>();
        _taskListDbSetMock = new Mock<DbSet<TaskList>>();
        _mockRepository = new TaskListRepository(_dbContextMock.Object);
        _dbContextMock.Setup(m => m.TaskLists).Returns(_taskListDbSetMock.Object);
    }


    private TaskManagerContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseInMemoryDatabase(databaseName: "TaskManagerTestDb")
            .Options;

        return new TaskManagerContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddTaskListAndSaveChanges()
    {
        // Arrange
        var taskList = new TaskList
        {
            Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a",
            Name = "New TaskList"
        };

        // Act
        await _mockRepository.CreateAsync(taskList);

        // Assert
        _taskListDbSetMock
            .Verify(m => m.AddAsync(taskList, It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock
            .Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnTaskList_WhenTaskListExists()
    {
        // Arrange
        var taskListId = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a";
        var taskList = new TaskList
        {
            Id = taskListId,
            Name = "Test TaskList",
            TaskItems = []
        };

        using var context = GetInMemoryDbContext();
        context.TaskLists.Add(taskList);
        context.SaveChanges();

        var repository = new TaskListRepository(context);

        // Act
        TaskList? result = await repository.GetAsync(taskListId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(taskListId);
        result.Name.Should().Be(taskList.Name);
        result.TaskItems.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTaskLists_WhenAnyTaskListExists()
    {
        // Arrange
        var taskLists = new List<TaskList>() 
        {
           TaskList.Create("Gym Tasks", Guid.NewGuid().ToString()),
           TaskList.Create("School Tasks", Guid.NewGuid().ToString()),
           TaskList.Create("Vacation Tasks", Guid.NewGuid().ToString())
        };

        using var context = GetInMemoryDbContext();
        context.TaskLists.AddRange(taskLists);
        context.SaveChanges();

        var repository = new TaskListRepository(context);

        // Act
        IEnumerable<TaskList>? result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        result.Should().BeOfType<List<TaskList>>();
    }

    [Fact]
    public void Delete_ShouldRemoveTaskList_WhenTaskListExists()
    {
        // Arrange
        var taskList = new TaskList
        {
            Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a",
            Name = "New TaskList"
        };
        // Act
        _mockRepository.DeleteAsync(taskList);
        // Assert
        _taskListDbSetMock.Verify(m => m.Remove(taskList), Times.Once);
        _dbContextMock.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_UpdateTaskList()
    {
        TaskList taskList = new()
        {
            Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a",
            Name = "Test Task",
            LastUpdatedAt = DateTime.Now
        };

        var newName = "Task Test";

        // Act
        var result = await _mockRepository.UpdateAsync(taskList, newName);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(newName);
        _dbContextMock
            .Verify(m => m
            .SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}
