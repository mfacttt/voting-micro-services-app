using FluentAssertions;
using Moq;
using TallyService.Application.Services;
using TallyService.Domain.Entities;
using TallyService.Domain.Repositories;
using Xunit;

public class TallyServiceFinalizeElectionTests
{
    [Fact]
    public async Task FinalizeElectionAsync_ShouldMarkAllVoteCountsAsFinal_AndSaveChanges()
    {
        // Arrange
        var repositoryMock = new Mock<IVoteCountRepository>(MockBehavior.Strict);

        var electionId = Guid.NewGuid();

        var voteCounts = new List<VoteCount>
        {
            new VoteCount
            {
                ElectionId = electionId,
                CandidateId = Guid.NewGuid(),
                Count = 10
            },
            new VoteCount
            {
                ElectionId = electionId,
                CandidateId = Guid.NewGuid(),
                Count = 25
            }
        };

        repositoryMock
            .Setup(r => r.GetByElectionAsync(electionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(voteCounts);

        repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new TallyService.Application.Services.TallyService(repositoryMock.Object);

        // Act
        await service.FinalizeElectionAsync(electionId, CancellationToken.None);

        // Assert — domain state
        voteCounts.Should().AllSatisfy(v =>
        {
            v.IsFinal.Should().BeTrue();
        });

        // Assert — repository calls
        repositoryMock.Verify(
            r => r.GetByElectionAsync(electionId, It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}