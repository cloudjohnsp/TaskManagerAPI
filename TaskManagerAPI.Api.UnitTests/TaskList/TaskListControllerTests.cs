using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Api.Controllers;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Api.UnitTests;

public class TaskListControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TaskList _taskList;
    private readonly TaskListResponse _taskListResponse;
    private readonly TaskListController _taskListController;

    public TaskListControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _taskList = TaskList.Create("Test List", Guid.NewGuid().ToString());
        _taskListResponse = new TaskListResponse(
            _taskList.Id,
            _taskList.Name,
            _taskList.CreatedAt,
            _taskList.LastUpdatedAt,
            [],
            _taskList.UserId
        );
        _taskListController = new TaskListController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateTaskList_ShouldReturn201Created_WhenTaskListIsCreated()
    {
        // Arrange
        var request = new CreateTaskListRequest(_taskList.Name, _taskList.UserId);
        var command = new CreateTaskListCommand(request.Name, request.UserId);

        _mapperMock.Setup(m => m.Map<CreateTaskListCommand>(request))
            .Returns(command);
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskList);
        _mapperMock.Setup(m => m.Map<TaskListResponse>(_taskList))
            .Returns(_taskListResponse);

        // Act
        var result = await _taskListController.CreateTaskList(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

        var resultValue = actionResult.Value as TaskListResponse;
        resultValue.Should().BeEquivalentTo(_taskListResponse);

        _mapperMock.Verify(m => m.Map<CreateTaskListCommand>(request), Times.Once);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskListResponse>(_taskList), Times.Once);
    }

    [Fact]
    public async Task GetTaskList_ShouldReturn200OK_WhenTaskListIsFound()
    {
        // Arrange

        _mediatorMock.Setup(m => m.Send(It.Is<GetTaskListQuery>(q => q.Id == _taskList.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskList);
        _mapperMock.Setup(m => m.Map<TaskListResponse>(_taskList))
            .Returns(_taskListResponse);

        // Act
        var result = await _taskListController.GetTaskList(_taskList.Id);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as TaskListResponse;
        resultValue.Should().BeEquivalentTo(_taskListResponse);

        _mediatorMock.Verify(m => m.Send(It.Is<GetTaskListQuery>(q => q.Id == _taskList.Id), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskListResponse>(_taskList), Times.Once);
    }

    [Fact]
    public async Task GetAllTaskLists_ShouldReturn200OK_WhenTaskListsAreFound()
    {
        // Arrange
        string listId = Guid.NewGuid().ToString();

        var taskLists = new List<TaskList> {
            TaskList.Create("Test List", listId),
            TaskList.Create("Test Lis", listId),
            TaskList.Create("Test Li", listId),
        };

        var response = new List<TaskListResponse>
        {
            new TaskListResponse(
                taskLists[0].Id,
                taskLists[0].Name,
                taskLists[0].CreatedAt,
                taskLists[0].LastUpdatedAt,
                [],
                taskLists[0].UserId
            ),
            new TaskListResponse(
                taskLists[1].Id,
                taskLists[1].Name,
                taskLists[1].CreatedAt,
                taskLists[1].LastUpdatedAt,
                [],
                taskLists[1].UserId
            ),
            new TaskListResponse(
                taskLists[2].Id,
                taskLists[2].Name,
                taskLists[2].CreatedAt,
                taskLists[2].LastUpdatedAt,
                [],
                taskLists[2].UserId
            )
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTaskListsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskLists);
        _mapperMock.Setup(m => m.Map<IEnumerable<TaskListResponse>>(taskLists))
            .Returns(response);

        // Act
        var result = await _taskListController.GetAllTaskLists();

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as IEnumerable<TaskListResponse>;
        resultValue.Should().BeEquivalentTo(response);

        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllTaskListsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<TaskListResponse>>(taskLists), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskList_ShouldReturn200OK_WhenTaskListIsUpdated()
    {
        // Arrange
        var request = new UpdateTaskListRequest(Guid.NewGuid().ToString(), "Updated List");
        var command = new UpdateTaskListCommand(request.Id, request.Name);

        _mapperMock.Setup(m => m.Map<UpdateTaskListCommand>(request))
            .Returns(command);
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_taskList);
        _mapperMock.Setup(m => m.Map<TaskListResponse>(_taskList))
            .Returns(_taskListResponse);

        // Act
        var result = await _taskListController.UpdateTaskList(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as TaskListResponse;
        resultValue.Should().BeEquivalentTo(_taskListResponse);

        _mapperMock.Verify(m => m.Map<UpdateTaskListCommand>(request), Times.Once);
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<TaskListResponse>(_taskList), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskList_ShouldReturn204NoContent_WhenTaskListIsDeleted()
    {
        // Arrange
        var command = new DeleteTaskListCommand(_taskList.Id);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        var result = await _taskListController.DeleteTaskList(_taskList.Id);

        // Assert
        var statusResult = result as StatusCodeResult;
        statusResult.Should().NotBeNull();
        statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }


}
