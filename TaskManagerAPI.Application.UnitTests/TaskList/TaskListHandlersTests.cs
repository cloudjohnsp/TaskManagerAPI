using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Handlers.Commands;
using TaskManagerAPI.Application.Handlers.Queries;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.UnitTests;

public class TaskListHandlersTests
{
    private readonly Mock<ITaskListRepository> _taskListRepositoryMock = null!;
    public TaskListHandlersTests()
    {
        _taskListRepositoryMock = new Mock<ITaskListRepository>();
    }

    [Fact]
    public async Task CreateTaskListCommandHandler_Returns_TaskList()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var taskList = TaskList.Create("Test Tasks", userId);

        _taskListRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<TaskList>()))
            .Verifiable();
        _taskListRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskList?)taskList));

        CreateTaskListCommand command = new(taskList.Name, taskList.UserId);
        CreateTaskListCommandHandler handler = new(_taskListRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskList>();
        result.Name.Should().Be(taskList.Name);
        result.Id.Should().Be(taskList.Id);
    }

    [Fact]
    public async void GetTaskListQueryHandler_Returns_TaskList()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var taskList = TaskList.Create("Test Tasks", userId);

        _taskListRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskList?)taskList));

        GetTaskListQuery query = new(taskList.Id);
        GetTaskListQueryHandler handler = new(_taskListRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Should().NotBeNull();
        result.Should().BeOfType<TaskList>();
        result?.Id.Should().Be(taskList.Id);
        result?.Name.Should().Be(taskList.Name);
    }

    [Fact]
    public async void GetAllTaskListQueryHandler_Returns_AllTaskLists()
    {
        var taskLists = new List<TaskList>()
        {
            TaskList.Create("Test Tasks", Guid.NewGuid().ToString()),
            TaskList.Create("Tes Task", Guid.NewGuid().ToString()),
            TaskList.Create("Te Tas", Guid.NewGuid().ToString())
        };

        // Arrange
        _taskListRepositoryMock
            .Setup(x => x.GetAllAsync())
            .Returns(Task.FromResult((IEnumerable<TaskList>?)taskLists));

        GetAllTaskListsQuery query = new();
        GetAllTaskListsQueryHandler handler = new(_taskListRepositoryMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async void DeleteTaskListCommandHandler_Returns_NothingOnSuccess()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var taskList = TaskList.Create("Test Tasks", userId);

        _taskListRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskList?)taskList));

        _taskListRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<TaskList>()))
            .Verifiable();

        DeleteTaskListCommand command = new(taskList.Id);
        DeleteTaskListCommandHandler handler = new(_taskListRepositoryMock.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _taskListRepositoryMock
            .Verify(x => x.DeleteAsync(It.IsAny<TaskList>()), Times.Once);
    }

    [Fact]
    public async void UpdateTaskListCommandHandler_Returns_TaskList()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var taskList = TaskList.Create("Test Tasks", userId);
        string newName = "Tasks Tests";

        _taskListRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskList?)taskList));

        _taskListRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<TaskList>(), It.IsAny<string>()))
            .Returns(Task.FromResult((TaskList?)taskList));

        taskList.Name = newName;

        UpdateTaskListCommand command = new(taskList.Id, newName);
        UpdateTaskListCommandHandler handler = new(_taskListRepositoryMock.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskList>();
        result?.Name.Should().Be(newName);
    }
}
