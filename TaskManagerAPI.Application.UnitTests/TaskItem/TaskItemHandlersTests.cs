using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
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

public class TaskItemHandlersTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepository;
    public TaskItemHandlersTests()
    {
        _taskItemRepository = new Mock<ITaskItemRepository>();
    }

    [Fact]
    public async Task CreateTaskItemCommandHandler_Returns_TaskItemOnSuccess()
    {
        // Arrange
        string listId = Guid.NewGuid().ToString();
        var taskItem = TaskItem.Create("Test Item", listId);
        _taskItemRepository.Setup(x => x.CreateAsync(It.IsAny<TaskItem>()))
            .Verifiable();
        _taskItemRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskItem?)taskItem));

        CreateTaskItemCommand command = new("Test Item", listId);
        CreateTaskItemCommandHandler handler = new(_taskItemRepository.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskItem>();
        result.Description.Should().Be(taskItem.Description);
    }

    [Fact]
    public async Task GetTaskItemQueryHandler_Returns_TaskItemWhenExists()
    {
        // Arrange
        string listId = Guid.NewGuid().ToString();
        var taskItem = TaskItem.Create("Test Item", listId);

        _taskItemRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskItem?)taskItem));
        GetTaskItemQuery query = new(taskItem.Id);
        GetTaskItemQueryHandler handler = new(_taskItemRepository.Object);
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskItem>();
        result?.Id.Should().Be(taskItem.Id);
        result?.TaskListId.Should().Be(listId);
        result?.Description.Should().Be(taskItem.Description);
    }

    [Fact]
    public async Task UpdateTaskItemCommandHandler_Returns_UpdatedTaskItem()
    {
        // Arrange
        string listId = Guid.NewGuid().ToString();
        string newDescription = "Item Test";
        var taskItem = TaskItem.Create("Test Item", listId);

        _taskItemRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskItem?)taskItem));

        _taskItemRepository
            .Setup(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<string>()))
            .Returns(Task.FromResult(taskItem));

        taskItem.Description = newDescription;

        UpdateTaskItemCommand command = new(taskItem.Id, newDescription);
        UpdateTaskItemCommandHandler handler = new(_taskItemRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TaskItem>();
        result?.Description.Should().Be(newDescription);
        result?.TaskListId.Should().Be(listId);
    }

    [Fact]
    public async void DeleteTaskItemCommandHandler_Returns_NothingOnSuccess()
    {
        // Arrange
        string listId = Guid.NewGuid().ToString();
        var taskItem = TaskItem.Create("Test Item", listId);

        _taskItemRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((TaskItem?)taskItem));
        _taskItemRepository
            .Setup(x => x.DeleteAsync(It.IsAny<TaskItem>()))
            .Verifiable();

        DeleteTaskItemCommand command = new(taskItem.Id);
        DeleteTaskItemCommandHandler handler = new(_taskItemRepository.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _taskItemRepository
            .Verify(x => x.DeleteAsync(It.IsAny<TaskItem>()), Times.Once);
    }

}
