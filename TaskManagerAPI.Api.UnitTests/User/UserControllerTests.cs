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
using TaskManagerAPI.Contracts;
using TaskManagerAPI.Contracts.HTTP;
using TaskManagerAPI.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManagerAPI.Api.UnitTests;

public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _userController = new UserController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateUser_ShouldReturn201Created_WhenUserIsCreated()
    {
        // Arrange
        var request = new CreateUserRequest("testUser", "password123", "Common");
        var command = new CreateUserCommand(request.NickName, request.Password, request.Role);
        var user = User.Create(command.NickName, command.Password, command.Role);
        var response = new CreateUserResponse(user.Id, user.NickName, user.Role, user.CreatedAt, user.LastUpdatedAt);

        _mediatorMock
            .Setup(m => m.Send(It.Is<CreateUserCommand>(c => c.NickName == request.NickName),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<CreateUserResponse>(user))
            .Returns(response);

        // Act
        var result = await _userController.CreateUser(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

        var resultValue = actionResult.Value as CreateUserResponse;
        resultValue.Should().BeEquivalentTo(response);

        _mediatorMock
            .Verify(m => m.Send(It.Is<CreateUserCommand>(c => c.NickName == request.NickName),
                It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CreateUserResponse>(user), Times.Once);
    }

    [Fact]
    public async Task GetUser_ShouldReturn200OK_WhenUserIsFound()
    {
        // Arrange
        var user = User.Create("testUser", "password123", "Common");
        var response = new UserResponse(user.Id, user.NickName, user.Role, user.CreatedAt, user.LastUpdatedAt, []);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetUserQuery>(q => q.Id == user.Id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

        _mapperMock.Setup(m => m.Map<UserResponse>(user))
            .Returns(response);

        // Act
        var result = await _userController.GetUser(user.Id);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as UserResponse;
        resultValue.Should().BeEquivalentTo(response);

        _mediatorMock
            .Verify(m => m.Send(It.Is<GetUserQuery>(q => q.Id == user.Id), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponse>(user), Times.Once);
    }

    [Fact]
    public async Task UpdateNickName_ShouldReturn200OK_WhenNickNameIsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var request = new UpdateNickNameRequest(userId, "new_name");
        var command = new UpdateUserNickNameCommand(request.Id, request.NickName);
        var user = User.Create(command.NickName, "Pass@1234", "Admin");
        user.Id = userId;
        var response = new UserResponse(
            user.Id,
            user.NickName,
            user.Role,
            user.CreatedAt,
            user.LastUpdatedAt,
            []
        );

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user))
            .Returns(response);

        // Act
        var result = await _userController.UpdateNickName(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as UserResponse;
        resultValue.Should().BeEquivalentTo(response);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponse>(user), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_ShouldReturn204NoContent_WhenPasswordIsUpdated()
    {
        // Arrange
        var request = new UpdatePasswordRequest("test-id", "newPassword123");
        var command = new UpdateUserPasswordCommand(request.Id, request.Password);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        var result = await _userController.UpdatePassword(request);

        // Assert
        var statusResult = result as StatusCodeResult;
        statusResult.Should().NotBeNull();
        statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturn204NoContent_WhenUserIsDeleted()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var command = new DeleteUserCommand(id);

        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        var result = await _userController.DeleteUser(id);

        // Assert
        var statusResult = result as StatusCodeResult;
        statusResult.Should().NotBeNull();
        statusResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturn200OK_WhenLoginIsSuccessful()
    {
        // Arrange
        var request = new LoginRequest("testUser", "password123");
        var query = new LoginQuery(request.NickName, request.Password);
        var user = User.Create(request.NickName, request.Password, "Common");
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        var loginResult = new LoginResult(user, token);
        var response = new LoginResponse(user.Id, user.NickName, user.Role, token);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loginResult);
        _mapperMock.Setup(m => m.Map<LoginResponse>(loginResult))
            .Returns(response);

        // Act
        var result = await _userController.Login(request);

        // Assert
        var actionResult = result.Result as ObjectResult;
        actionResult.Should().NotBeNull();
        actionResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var resultValue = actionResult.Value as LoginResponse;
        resultValue.Should().BeEquivalentTo(response);

        _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<LoginResponse>(loginResult), Times.Once);
    }


}
