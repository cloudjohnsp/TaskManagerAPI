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
    //private readonly User _user = null!;
    //private readonly TaskList _taskList = null!;
    //private readonly Mock<ITaskListRepository> _taskListRepositoryMock = null!;
    //private readonly Fixture _fixture = null!;
    //public TaskListHandlersTests()
    //{
    //    _user = User.Create("john_doe", "Password@01234", "User");
    //    _taskList = TaskList.Create("Test Tasks", _user.Id);
    //    _taskListRepositoryMock = new Mock<ITaskListRepository>();
    //    _fixture = new Fixture();
    //    _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
    //    _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    //}

    ////[Fact]
    ////public async void CreateAsync_Returns_TaskList()
    ////{
        
    ////}

    //[Fact]
    //public async void GetAsync_Returns_TaskList()
    //{
    //    // Arrange
    //    _taskListRepositoryMock
    //        .Setup(x => x.GetAsync(It.IsAny<string>()))
    //        .Returns(Task.FromResult((TaskList?)_taskList));
    //    GetTaskListQuery query = new(_taskList.Id);
    //    GetTaskListQueryHandler handler = new(_taskListRepositoryMock.Object);
    //    // Act
    //    var result = await handler.Handle(query, CancellationToken.None);
    //    // Assert
    //    result?.Should().NotBeNull();
    //    result.Should().BeOfType<TaskList>();
    //    result?.Id.Should().Be(_taskList.Id);
    //}

    //[Fact]
    //public async void GetAllAsync_Returns_AllTaskLists()
    //{
    //    // Arrange
    //    IEnumerable<TaskList> sut = _fixture.CreateMany<TaskList>(3);
    //    _taskListRepositoryMock
    //        .Setup(x => x.GetAllAsync())
    //        .Returns(Task.FromResult((IEnumerable<TaskList>?)sut));
    //    GetAllTaskListsQuery query = new();
    //    GetAllTaskListsQueryHandler handler = new(_taskListRepositoryMock.Object);
    //    // Act
    //    var result = await handler.Handle(query, CancellationToken.None);
    //    // Assert
    //    result.Should().NotBeEmpty();
    //    result.Should().HaveCount(3);
    //}

    //[Fact]
    //public async void DeleteAsync_Returns_Nothing()
    //{
    //    // Arrange
    //    _taskListRepositoryMock
    //        .Setup(x => x.DeleteAsync(It.IsAny<string>()))
    //        .Returns(Task.CompletedTask);
    //    DeleteTaskListCommand command = new(_taskList.Id);
    //    DeleteTaskListCommandHandler handler = new(_taskListRepositoryMock.Object);
    //    // Act
    //    var result = await handler.Handle(command, CancellationToken.None);
    //    // Assert
    //    result?.Should().Be(Task.CompletedTask);

    //}

    //[Fact]
    //public async void UpdateAsync_Returns_TaskList()
    //{
    //    _taskList.Name = "Test";
    //    // Arrange
    //    _taskListRepositoryMock
    //        .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<string>()))
    //        .Returns(Task.FromResult((TaskList?)_taskList));
    //    UpdateTaskListCommand command = new(_taskList.Id, "Test");
    //    UpdateTaskListCommandHandler handler = new(_taskListRepositoryMock.Object);
    //    // Act
    //    var result = await handler.Handle(command, CancellationToken.None);
    //    // Assert
    //    result.Should().NotBeNull();
    //    result.Should().BeOfType<TaskList>();
    //    result?.Name.Should().Be("Test");
    //}
}
