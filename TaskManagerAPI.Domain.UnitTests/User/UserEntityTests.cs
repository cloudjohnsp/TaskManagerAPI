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
    public void CreateMethod_Returns_ANewUserEntity()
    {
        // Arrange
        // Act
        User result = User.Create("john_doe", "Password@01234");
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<User>();
        result.NickName.Should().Be("john_doe");
    }
}
