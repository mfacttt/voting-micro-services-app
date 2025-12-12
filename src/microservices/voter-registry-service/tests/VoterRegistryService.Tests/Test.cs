using FluentAssertions;
using Moq;
using VoterRegistryService.Application.Services;
using VoterRegistryService.Domain.Entities;
using VoterRegistryService.Domain.Enums;
using VoterRegistryService.Domain.Repositories;
using Xunit;

namespace VoterRegistryService.Tests;

public class VoterServiceGetByIdTests
{
    [Fact]
    public async Task GetByIdAsync_WhenVoterExists_ShouldReturnMappedVoterResponse()
    {
        // Arrange
        var repositoryMock = new Mock<IVoterRepository>(MockBehavior.Strict);

        var voterId = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;

        var voter = new Voter
        {
            Id = voterId,
            NationalId = "123456789",
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Country = "MD",
            Address = "Chisinau",
            IsResident = true,
            CreatedAt = createdAt
        };

        voter.Activate(); // Status = Active

        repositoryMock
            .Setup(r => r.GetByIdAsync(voterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(voter);

        var service = new VoterService(repositoryMock.Object);

        // Act
        var result = await service.GetByIdAsync(voterId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        result!.Id.Should().Be(voter.Id);
        result.NationalId.Should().Be(voter.NationalId);
        result.FirstName.Should().Be(voter.FirstName);
        result.LastName.Should().Be(voter.LastName);
        result.DateOfBirth.Should().Be(voter.DateOfBirth);
        result.Country.Should().Be(voter.Country);
        result.Address.Should().Be(voter.Address);
        result.IsResident.Should().Be(voter.IsResident);
        result.Status.Should().Be(VoterStatus.Active);
        result.CreatedAt.Should().Be(createdAt);

        repositoryMock.Verify(
            r => r.GetByIdAsync(voterId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
