using FluentAssertions;
using IdentityService.Application.Contracts;
using IdentityService.Application.Services;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using Moq;

namespace IdentityService.Tests;

public class AuthServiceLoginTests
{
    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnSuccessMessage()
    {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

        var user = new User
        {
            Email = "test@example.com",
            Password = "password123"
        };

        repositoryMock
            .Setup(r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var authService = new AuthService(repositoryMock.Object);

        var request = new LoginRequest(
            Email: "test@example.com",
            Password: "password123"
        );

        // Act
        var result = await authService.Login(request, CancellationToken.None);

        // Assert
        result.Should().Be("Login successful.");

        repositoryMock.Verify(
            r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}