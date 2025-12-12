using ElectionService.Application.Contracts.Elections;
using ElectionService.Domain.Entities;
using ElectionService.Domain.Enums;
using ElectionService.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace ElectionService.Tests;

public class ElectionServiceCreateAsyncTests
{
    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateScheduledElectionAndReturnResponse()
    {
        // Arrange
        var repositoryMock = new Mock<IElectionRepository>(MockBehavior.Strict);

        Election? capturedElection = null;

        repositoryMock
            .Setup(r => r.Add(It.IsAny<Election>(), It.IsAny<CancellationToken>()))
            .Callback<Election, CancellationToken>((e, _) => capturedElection = e);

        repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new ElectionService.Application.Services.ElectionService.ElectionService(repositoryMock.Object);

        var startsAt = DateTimeOffset.UtcNow.AddDays(1);
        var endsAt = DateTimeOffset.UtcNow.AddDays(2);

        var request = new CreateElectionsRequest(
            Name: "Presidential Elections",
            Description: "2025 elections",
            StartsAtUtc: startsAt,
            EndsAtUtc: endsAt
        );

        // Act
        var response = await service.CreateAsync(request);

        // Assert — repository calls
        repositoryMock.Verify(
            r => r.Add(It.IsAny<Election>(), It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        // Assert — entity mapping
        capturedElection.Should().NotBeNull();
        capturedElection!.Id.Should().NotBe(Guid.Empty);
        capturedElection.Name.Should().Be(request.Name);
        capturedElection.Description.Should().Be(request.Description);
        capturedElection.StartsAtUtc.Should().Be(startsAt);
        capturedElection.EndsAtUtc.Should().Be(endsAt);

        capturedElection.Status.Should().Be(ElectionStatus.Scheduled);
        capturedElection.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
        capturedElection.UpdatedAt.Should().BeNull();
        capturedElection.Candidates.Should().BeEmpty();

        // Assert — response mapping
        response.Id.Should().Be(capturedElection.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.StartsAtUtc.Should().Be(startsAt);
        response.EndsAtUtc.Should().Be(endsAt);
        response.Status.Should().Be(ElectionStatus.Scheduled);
        response.CandidatesCount.Should().Be(0);
    }
}