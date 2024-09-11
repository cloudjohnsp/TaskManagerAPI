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
    public async Task CreateTaskItem_ReturnsStatus201_OnSuccess()
    {
        // Arrange
        var request = new CreateTaskItemRequest(_taskItem.Description, _taskItem.TaskListId);
        var command = new CreateTaskItemCommand(_taskItem.Description, _taskItem.TaskListId);

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
}
