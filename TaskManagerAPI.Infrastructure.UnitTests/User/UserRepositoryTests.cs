using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Application.Commands;
using TaskManagerAPI.Application.Handlers;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class UserRepositoryTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private User _user;

    public UserRepositoryTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _user = User.Create("john_doe", "Password#1090");
    }

    [Fact]
    public async void Create_Returns_User()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
            .Returns(Task.FromResult(_user));
        var command = new CreateUserCommand(_user.NickName, _user.Password);
        var handler = new CreateUserCommandHandler(_userRepositoryMock.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result.NickName.Should().Be("john_doe");
    }

    [Fact]
    public async void Get_Returns_User()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.Get(It.IsAny<string>()))
            .Returns(Task.FromResult((User?)_user));

        var query = new GetUserQuery(_user.Id);
        var handler = new GetUserQueryHandler(_userRepositoryMock.Object);
        // Act
        User? result = await handler.Handle(query, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result?.NickName.Should().Be("john_doe");
    }

    [Fact]
    public async void UpdateNickName_Returns_UpdatedUser()
    {
        // Arrange
        var updatedUser = User.Create("jane_doe", "Password#1090");
        updatedUser.Id = _user.Id;
        updatedUser.CreatedAt = _user.CreatedAt;
        updatedUser.LastUpdatedAt = _user.LastUpdatedAt;

        _userRepositoryMock
            .Setup(x => x.UpdateNickName(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult((User?)updatedUser));

        UpdateUserNickNameCommand command = new(_user.Id, updatedUser.NickName);
        UpdateUserNickNameCommandHandler handler = new(_userRepositoryMock.Object);

        // Act
        User? result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result?.NickName.Should().Be("jane_doe");
    }

    [Fact]
    public async void Delete_Returns_Nothing()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.Delete(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        DeleteUserCommand command = new(_user.Id);
        DeleteUserCommandHandler handler = new(_userRepositoryMock.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Task.CompletedTask);
    }

    [Fact]
    public async void UpdatePassword_Returns_Nothing()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.UpdatePassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        UpdateUserPasswordCommand command = new(_user.Id, "Password@01234");
        UpdateUserPasswordCommandHandler handler = new(_userRepositoryMock.Object);
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Task.CompletedTask);
    }
}
