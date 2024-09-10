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
    private readonly JwtSettings _jwtSettings;
    private readonly IOptions<JwtSettings> _optionsMock;

    public AuthMethodsTests()
    {
        _jwtSettings = new JwtSettings() 
        { 
            Audience = "test",
            Issuer = "test",
            ExpiryMinutes = 60,
            Secret = "kxcKREQtwQgUnLoxtugQQiWIQr9vZy3b"
        };
        _optionsMock = Options.Create<JwtSettings>(_jwtSettings);
    }

    [Fact]
    public void GenerateToken_Returns_TokenOnSuccess()
    {
        // Arrange
        var user = User.Create("john_doe", "Pass@1234", "Common");
        JwtGenerator jwtGenerator = new(_optionsMock);
        // Act
        var result = jwtGenerator.GenerateToken(user);
        // Assert
        result.Should().BeOfType<string>();
    }
}
