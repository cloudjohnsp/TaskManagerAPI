using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Api.Controllers;
using TaskManagerAPI.Application.Queries;
using TaskManagerAPI.Contracts;
using TaskManagerAPI.Domain.Entities;
using MapsterMapper;
using MediatR;
using FluentAssertions;
using TaskManagerAPI.Contracts.HTTP;

namespace TaskManagerAPI.Api.UnitTests;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _authController = new AuthController(_mediatorMock.Object, _mapperMock.Object);
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
        var result = await _authController.Login(request);

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
