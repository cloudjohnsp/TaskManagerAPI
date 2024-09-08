using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Persistence.EntityFramework.Contexts;
using TaskManagerAPI.Infrastructure.Persistence.Repositories;

namespace TaskManagerAPI.Infrastructure.UnitTests;

public class UserRepositoryTests
{
    private readonly Mock<TaskManagerContext> _dbContextMock;
    private readonly Mock<DbSet<User>> _userDbSetMock;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _dbContextMock = new Mock<TaskManagerContext>();
        _userDbSetMock = new Mock<DbSet<User>>();
        _dbContextMock.Setup(m => m.Users).Returns(_userDbSetMock.Object);
        _userRepository = new UserRepository(_dbContextMock.Object);
    }

    private TaskManagerContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TaskManagerContext>()
            .UseInMemoryDatabase(databaseName: "TaskManagerTestDb")
            .Options;

        return new TaskManagerContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUserAndSaveChanges()
    {
        // Arrange
        var user = new User { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", NickName = "test_user" };

        // Act
        await _userRepository.CreateAsync(user);

        // Assert
        _userDbSetMock.Verify(x => x.AddAsync(user, default), Times.Once);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public void DeleteAsync_ShouldRemoveUserAndSaveChanges()
    {
        // Arrange
        var user = new User { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", NickName = "test_user" };

        // Act
        _userRepository.DeleteAsync(user);

        // Assert
        _userDbSetMock.Verify(x => x.Remove(user), Times.Once);
        _dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a";
        var user = new User { Id = userId, NickName = "test_user" };

        using var context = GetInMemoryDbContext();
        context.Users.Add(user);
        context.SaveChanges();

        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task GetByNickNameAsync_ShouldReturnUser_WhenNickNameExists()
    {
        var users = User.Create("john_doe", "Pass@1234", "Common");
        // Arrange
        using var context = GetInMemoryDbContext();
        context.Users.Add(users);
        context.SaveChanges();

        var repository = new UserRepository(context);
        // Act
        var result = await repository.GetByNickNameAsync("john_doe");

        // Assert
        result.Should().NotBeNull();
        result!.NickName.Should().Be("john_doe");
        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task UpdateNickNameAsync_ShouldUpdateNickNameAndSaveChanges()
    {
        // Arrange
        var userId = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a";
        var user = new User { Id = userId, NickName = "old_nickname" };
        var newNickName = "new_nickname";

        // Act
        var result = await _userRepository.UpdateNickNameAsync(user, newNickName);

        // Assert
        result.NickName.Should().Be(newNickName);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ShouldUpdatePasswordAndSaveChanges()
    {
        // Arrange
        var userId = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a";
        var user = new User { Id = userId, Password = "old_password" };
        var newPassword = "new_password";

        // Act
        await _userRepository.UpdatePasswordAsync(user, newPassword);

        // Assert
        user.Password.Should().Be(newPassword);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
