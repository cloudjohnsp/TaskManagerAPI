using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Exceptions;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class TaskItemRepositoryTests
{
    private readonly Mock<TaskManagerContext> _dbContextMock;
    private readonly Mock<DbSet<TaskItem>> _taskItemsDbSetMock;
    private readonly TaskItemRepository _repository;

    public TaskItemRepositoryTests()
    {
        _dbContextMock = new Mock<TaskManagerContext>();
        _taskItemsDbSetMock = new Mock<DbSet<TaskItem>>();
        _repository = new TaskItemRepository(_dbContextMock.Object);
        _dbContextMock.Setup(m => m.TaskItems).Returns(_taskItemsDbSetMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedTaskItem()
    {
        // Arrange
        var taskItem = new TaskItem { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", Description = "Test Task" };

        // Act
        await _repository.CreateAsync(taskItem);

        // Assert
        _taskItemsDbSetMock.Verify(m => m.AddAsync(taskItem, default), Times.Once);
        _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public void Delete_ShouldRemoveTaskItem_WhenTaskItemExists()
    {
        // Arrange
        var taskItem = new TaskItem { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", Description = "Test Task" };

        // Act
        _repository.DeleteAsync(taskItem);

        // Assert
        _taskItemsDbSetMock.Verify(m => m.Remove(taskItem), Times.Once);
        _dbContextMock.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnTaskItem_WhenTaskItemExists()
    {
        // Arrange
        var taskItem = new TaskItem { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", Description = "Test Task" };
        _taskItemsDbSetMock.Setup(m => m.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(taskItem);

        // Act
        var result = await _repository.GetAsync(taskItem.Id);

        // Assert
        result.Should().Be(taskItem);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_UpdateTaskItem()
    {
        TaskItem taskItem = new()
        {
            Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a",
            Description = "Test Task",
            LastUpdatedAt = DateTime.Now
        };

        var newDescription = "Task Test";

        // Act
        var result = await _repository.UpdateAsync(taskItem, newDescription);

        // Assert
        result.Should().NotBeNull();
        result.Description.Should().Be(newDescription);
        _dbContextMock
            .Verify(m => m
            .SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
