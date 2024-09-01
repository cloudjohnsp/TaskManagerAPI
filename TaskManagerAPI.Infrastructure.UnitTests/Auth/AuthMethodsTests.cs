using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Auth;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class AuthMethodsTests
{
    private readonly User _user = null!;
    private readonly Mock<IUserRepository> _userRepositoryMock = null!;
    private readonly JwtSettings _jwtSettingsMock = null!;
    private readonly Mock<IOptions<JwtSettings>> _optionsMock = null!;
    private JwtConfig _jwtConfigMock = null!;

    public AuthMethodsTests()
    {
        _user = User.Create("john_doe", "Password#1090");
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtSettingsMock = new JwtSettings() { Secret = "my-super-secret-keymy-super-secret-key" };
        _optionsMock = new Mock<IOptions<JwtSettings>>();
    }

    [Fact]
    public async void GenerateJwtToken_Returns_Token()
    {
        // Arrange
        _optionsMock.Setup(x => x.Value).Returns(_jwtSettingsMock);
        _jwtConfigMock = new(_optionsMock.Object, _userRepositoryMock.Object);
        // Act
        string token = await _jwtConfigMock.GenerateJwtToken(_user);
        // Assert
        token.Should().NotBeNull();
        token.Should().BeOfType<string>();
    }

    [Fact]
    public async void GetUserFromClaims_Returns_User()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(Task.FromResult((User?)_user));
        _optionsMock.Setup(x => x.Value).Returns(_jwtSettingsMock);
        _jwtConfigMock = new(_optionsMock.Object, _userRepositoryMock.Object);
        // Act
        string token = await _jwtConfigMock.GenerateJwtToken(_user);
        var result = await _jwtConfigMock.GetUserFromClaims(token);
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
    }
}
