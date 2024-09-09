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

public sealed class UserRepositoryTests : IDisposable
{
    private readonly Mock<TaskManagerContext> _dbContextMock;
    private readonly TaskManagerContext _inMemoryContext;
    private readonly Mock<DbSet<User>> _userDbSetMock;
    private readonly UserRepository _mockRepository;
    private readonly UserRepository _InMemoryRepository;

    public UserRepositoryTests()
    {
        _dbContextMock = new Mock<TaskManagerContext>();
        _userDbSetMock = new Mock<DbSet<User>>();
        _dbContextMock.Setup(m => m.Users).Returns(_userDbSetMock.Object);
        _mockRepository = new UserRepository(_dbContextMock.Object);
        _inMemoryContext = InMemoryDatabase.GetInMemoryDbContext();
        _InMemoryRepository = new UserRepository(_inMemoryContext);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddUserAndSaveChanges()
    {
        // Arrange
        var user = new User { Id = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a", NickName = "test_user" };

        // Act
        await _mockRepository.CreateAsync(user);

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
        _mockRepository.DeleteAsync(user);

        // Assert
        _userDbSetMock.Verify(x => x.Remove(user), Times.Once);
        _dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnUser_WhenUserExists()
    {
        InMemoryDatabase.InitializeDatabase(_inMemoryContext);
        // Arrange
        var userId = "eb1c5b8b-a26c-4551-a9d6-b6943ad9b50a";
        var user = new User { Id = userId, NickName = "test_user" };

        _inMemoryContext.Users.Add(user);
        _inMemoryContext.SaveChanges();

        // Act
        var result = await _InMemoryRepository.GetAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task GetByNickNameAsync_ShouldReturnUser_WhenNickNameExists()
    {
        InMemoryDatabase.InitializeDatabase(_inMemoryContext);

        var users = User.Create("john_doe", "Pass@1234", "Common");
        // Arrange
        _inMemoryContext.Users.Add(users);
        _inMemoryContext.SaveChanges();

        // Act
        var result = await _InMemoryRepository.GetByNickNameAsync("john_doe");

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
        var result = await _mockRepository.UpdateNickNameAsync(user, newNickName);

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
        await _mockRepository.UpdatePasswordAsync(user, newPassword);

        // Assert
        user.Password.Should().Be(newPassword);
        _dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    public void Dispose()
    {
        _inMemoryContext.Database.EnsureDeleted();
        _inMemoryContext.Dispose();
    }
}
