using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Handlers;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.UnitTests;

public class UserHandlersTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserHandlersTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async void CreateUserCommandHandler_Returns_User()
    {
        // Arrange
        var command = new CreateUserCommand
        (
            "new_user",
            "password123",
            "Common"
        );

        _userRepositoryMock.Setup(repo => repo.GetByNickNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .Verifiable();

        var createdUser = User.Create(command.NickName, BC.HashPassword(command.Password), command.Role);
        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(createdUser);

        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.NickName.Should().Be(command.NickName);
        _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void GetUserCommandHandler_Returns_UserWhenUserExists()
    {
        // Arrange
        var user = User.Create("john_doe", "Pass@1234", "Common");

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((User?)user));

        var query = new GetUserQuery(user.Id);
        var handler = new GetUserQueryHandler(_userRepositoryMock.Object);
        // Act
        User? result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result?.NickName.Should().Be("john_doe");
    }

    [Fact]
    public async void UpdateUserNickNameCommandHandler_Returns_UpdatedUserWhenUserExists()
    {
        // Arrange
        var updatedUser = User.Create("jane_doe", "Password#1090", "Common");
        string newNickName = "jannet_doe";

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)updatedUser);

        _userRepositoryMock
            .Setup(x => x.UpdateNickNameAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(updatedUser);

        updatedUser.NickName = newNickName;

        UpdateUserNickNameCommand command = new(updatedUser.Id, newNickName);
        UpdateUserNickNameCommandHandler handler = new(_userRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result?.NickName.Should().Be(newNickName);
    }

    [Fact]
    public async void DeleteUserCommandHandler_Returns_NothingWhenUserIsDeleted()
    {
        // Arrange
        var deletedUser = User.Create("jane_doe", "Password#1090", "Common");

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)deletedUser);

        _userRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<User>()))
            .Verifiable();

        DeleteUserCommand command = new(deletedUser.Id);

        DeleteUserCommandHandler handler = new(_userRepositoryMock.Object);
        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock
            .Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void UpdateUserPasswordCommandHandler_Returns_NothingWhenUpdated()
    {
        // Arrange
        var updatedUser = User.Create("jane_doe", "Password#1090", "Common");

        _userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)updatedUser);

        _userRepositoryMock
            .Setup(x => x.UpdatePasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Verifiable();

        UpdateUserPasswordCommand command = new(updatedUser.Id, "Password@01234");

        UpdateUserPasswordCommandHandler handler = new(_userRepositoryMock.Object);
        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock
            .Verify(x => x
            .UpdatePasswordAsync(It.IsAny<User>(), It.IsAny<string>()),
                Times.Once
            );
    }
}
