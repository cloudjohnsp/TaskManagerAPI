using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MapsterMapper;
using TaskManagerAPI.Api.Controllers;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using TaskManagerAPI.Application.Queries;

namespace TaskManagerAPI.Api.UnitTests;

public class TaskItemControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TaskItemController _taskItemController;
    private readonly TaskItem _taskItem;
    private readonly TaskItemResponse _taskItemResponse;
    public TaskItemControllerTests() 
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _taskItemController = new TaskItemController(_mediatorMock.Object, _mapperMock.Object);
        _taskItem = TaskItem.Create("Test Task", Guid.NewGuid().ToString());
        _taskItemResponse = new TaskItemResponse(
            _taskItem!.Id,
            _taskItem.Description,
            _taskItem.IsDone,
            _taskItem.CreatedAt,
            _taskItem.LastUpdatedAt,
            _taskItem.TaskListId
        );
    }

    [Fact]
    public async Task CreateTaskItem_ShouldReturnStatus201Created_WhenSuccessfull()
    {
        // Arrange
        var request = new CreateTaskItemRequest(_taskItem.Description, _taskItem.TaskListId);
        var command = new CreateTaskItemCommand(request.Description, request.TaskListId);

        _mapperMock.Setup(m => m.Map<CreateTaskItemCommand>(request))
            .Returns(command);
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskItem);
        _mapperMock.Setup(m => m.Map<TaskItemResponse>(_taskItem))
            .Returns(_taskItemResponse);

        // Act
        var result = await _taskItemController.CreateTaskItem(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status201Created);
        var resultValue = actionResult.Value as TaskItemResponse;
        resultValue.Should().BeEquivalentTo(_taskItemResponse);
        _mapperMock.Verify(m => m.Map<CreateTaskItemCommand>(request), Times.Once);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskItemResponse>(_taskItem), Times.Once);
    }

    [Fact]
    public async Task GetTaskItem_ShouldReturn200OK_WhenTaskItemIsFound()
    {
        // Arrange
        var query = new GetTaskItemQuery(_taskItem.Id);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskItem);
        _mapperMock.Setup(m => m.Map<TaskItemResponse>(_taskItem))
            .Returns(_taskItemResponse);

        // Act
        var result = await _taskItemController.GetTaskItem(query.Id);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as TaskItemResponse;
        resultValue.Should().BeEquivalentTo(_taskItemResponse);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskItemResponse>(_taskItem), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskItem_ShouldReturn200OK_WhenTaskItemIsUpdated()
    {
        // Arrange
        var request = new UpdateTaskItemRequest(_taskItem.Id, "New Description");
        var command = new UpdateTaskItemCommand(request.Id, request.Description);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskItem);
        _mapperMock.Setup(m => m.Map<TaskItemResponse>(_taskItem))
            .Returns(_taskItemResponse);

        // Act
        var result = await _taskItemController.UpdateTaskItem(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as TaskItemResponse;
        resultValue.Should().BeEquivalentTo(_taskItemResponse);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskItemResponse>(_taskItem), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskItemStatus_ShouldReturn200OK_WhenTaskItemStatusIsUpdated()
    {
        // Arrange
        var request = new UpdateTaskItemStatusRequest(_taskItem.Id, true);
        var command = new UpdateTaskItemStatusCommand(request.Id, request.IsDone);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskItem);
        _mapperMock.Setup(m => m.Map<TaskItemResponse>(_taskItem))
            .Returns(_taskItemResponse);

        // Act
        var result = await _taskItemController.UpdateTaskItemStatus(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as TaskItemResponse;
        resultValue.Should().BeEquivalentTo(_taskItemResponse);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskItemResponse>(_taskItem), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskItem_ShouldReturn204NoContent_WhenTaskItemIsDeleted()
    {
        // Arrange
        var command = new DeleteTaskItemCommand(_taskItem.Id);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        var result = await _taskItemController.DeleteTaskItem(command.Id);

        // Assert
        var statusResult = result as StatusCodeResult;
        statusResult.Should().NotBeNull();
        statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

}
