using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using TaskManagerAPI.Application.Handlers;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Auth;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Application.UnitTests;

public class AuthHandlersTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;

    public AuthHandlersTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtGeneratorMock = new Mock<IJwtGenerator>();
    }

    [Fact]
    public async Task LoginQueryHandler_WhenSuccessful_ReturnsUserAndToken()
    {
        // Arrange
        var query = new LoginQuery
        (
            "john_doe",
            "password123"
        );

        var user = User.Create("john_doe", BC.HashPassword("password123"), "Common");
        var token = "generated-jwt-token";

        _userRepositoryMock
            .Setup(repo => repo.GetByNickNameAsync(query.NickName))
            .ReturnsAsync(user);

        _jwtGeneratorMock
            .Setup(jwt => jwt.GenerateToken(user))
            .Returns(token);

        var handler = new LoginQueryHandler(_userRepositoryMock.Object, _jwtGeneratorMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.User.Should().Be(user);
        result.Token.Should().Be(token);
    }
}
