using FluentAssertions;
using Moq;
using VoteCastingService.Application.Contracts.Requests;
using VoteCastingService.Application.Services;
using VoteCastingService.Domain.Entities;
using VoteCastingService.Domain.Repositories;
using Xunit;

public class VoteCastingServiceTests
{
    [Fact]
    public async Task AddVoteAsync_WhenVoterHasNotVoted_ShouldAddVoteAndSave()
    {
        // Arrange
        var repositoryMock = new Mock<IVoteRepository>(MockBehavior.Strict);

        var electionId = Guid.NewGuid();
        var voterId = Guid.NewGuid();

        Vote? capturedVote = null;

        repositoryMock
            .Setup(r => r.HasVotedAsync(electionId, voterId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Vote>(), It.IsAny<CancellationToken>()))
            .Callback<Vote, CancellationToken>((v, _) => capturedVote = v);

        repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new VoteCastingService.Application.Services.VoteCastingService(
            repositoryMock.Object);

        var request = new CastVoteRequest(
            ElectionId: electionId,
            VoterId: voterId,
            CandidateId: Guid.NewGuid()
        );

        // Act
        await service.AddVoteAsync(request, CancellationToken.None);

        // Assert — repository calls
        repositoryMock.Verify(
            r => r.HasVotedAsync(electionId, voterId, It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Vote>(), It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        // Assert — mapped entity
        capturedVote.Should().NotBeNull();
        capturedVote!.ElectionId.Should().Be(electionId);
        capturedVote.VoterId.Should().Be(voterId);
        capturedVote.VoteAt.Should().BeCloseTo(
            DateTimeOffset.UtcNow,
            TimeSpan.FromSeconds(2));
    }
}
