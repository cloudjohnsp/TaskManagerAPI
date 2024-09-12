using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using FluentAssertions;

namespace TaskManagerAPI.Domain.UnitTests;

public class UserEntityTests
{
    [Fact]
    public void Create_Returns_ANewUserEntity()
    {
        // Arrange
        // Act
        User result = User.Create("john_doe", "Password@1234", "Common");
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result.NickName.Should().Be("john_doe");
        result.Password.Should().Be("Password@1234");
        result.Role.Should().Be("Common");
    }
}
