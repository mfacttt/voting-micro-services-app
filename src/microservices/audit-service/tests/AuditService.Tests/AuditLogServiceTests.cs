using AuditService.Application.Contracts.Requests;
using AuditService.Application.Services;
using AuditService.Domain.Abstractions;
using AuditService.Domain.Enums;
using FluentAssertions;
using Moq;

namespace AuditService.Tests;

public class AuditLogServiceTests
{
    private readonly Mock<IAuditLogRepository> _repositoryMock;
    private readonly AuditLogService _service;

    public AuditLogServiceTests()
    {
        _repositoryMock = new Mock<IAuditLogRepository>(MockBehavior.Strict);
        _service = new AuditLogService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAuditLogAsync_ShouldCallRepositoryWithMappedEntity()
    {
        // Arrange
        var request = new AuditLogRequest(
            ServiceName: "AuditService",
            ActionName: "CreateAuditLog",
            Message: "Audit log created",
            Severity: AuditSeverity.Info
        );

        _repositoryMock
            .Setup(r => r.AddAsync(
                It.Is<AuditLog>(log =>
                    log.ServiceName == request.ServiceName &&
                    log.ActionName == request.ActionName &&
                    log.Message == request.Message &&
                    log.Severity == request.Severity &&
                    log.Timestamp <= DateTimeOffset.UtcNow &&
                    log.Id != Guid.Empty
                ),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _service.CreateAuditLogAsync(request);

        // Assert
        _repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAuditLogsAsync_ShouldReturnMappedResponses()
    {
        // Arrange
        var logs = new List<AuditLog>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ServiceName = "ServiceA",
                ActionName = "ActionA",
                Message = "MessageA",
                Severity = AuditSeverity.Info,
                Timestamp = DateTimeOffset.UtcNow.AddMinutes(-5)
            },
            new()
            {
                Id = Guid.NewGuid(),
                ServiceName = "ServiceB",
                ActionName = "ActionB",
                Message = "MessageB",
                Severity = AuditSeverity.Warning,
                Timestamp = DateTimeOffset.UtcNow
            }
        };

        _repositoryMock
            .Setup(r => r.GetPagedAsync(
                 0,
                 2,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs)
            .Verifiable();

        // Act
        var result = await _service.GetAuditLogsAsync(pageNumber: 1, pageSize: 2);

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(logs[0].Id);
        result[0].ServiceName.Should().Be(logs[0].ServiceName);
        result[0].ActionName.Should().Be(logs[0].ActionName);
        result[0].Message.Should().Be(logs[0].Message);
        result[0].Severity.Should().Be(logs[0].Severity);
        result[0].Timestamp.Should().Be(logs[0].Timestamp);

        _repositoryMock.Verify();
    }

    [Theory]
    [InlineData(1, 10, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(3, 5, 10)]
    public async Task GetAuditLogsAsync_ShouldCalculateCorrectSkip(
        int pageNumber,
        int pageSize,
        int expectedSkip)
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetPagedAsync(
                expectedSkip,
                pageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<AuditLog>())
            .Verifiable();

        // Act
        await _service.GetAuditLogsAsync(pageNumber, pageSize);

        // Assert
        _repositoryMock.Verify();
    }
}
