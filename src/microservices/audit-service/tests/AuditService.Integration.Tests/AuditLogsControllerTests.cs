using System.Net;
using System.Net.Http.Json;
using AuditService.Application.Contracts.Requests;
using AuditService.Application.Contracts.Responses;
using AuditService.Domain.Enums;
using AuditService.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuditService.Integration.Tests;

public sealed class AuditLogsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuditLogsControllerTests(WebApplicationFactory<Program> factory)
    {
        var configuredFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuditDbContext>));

                if (dbDescriptor != null)
                {
                    services.Remove(dbDescriptor);
                }

                services.AddDbContext<AuditDbContext>(options => { options.UseInMemoryDatabase("AuditTestDb"); });
            });
        });

        _client = configuredFactory.CreateClient();
    }

    [Fact]
    public async Task CreateAuditLog_Then_GetPaged_ReturnsCreatedLog()
    {
        // Arrange
        var request = new AuditLogRequest(
            "identity-service",
            "UserLoggedIn",
            "Successfully logged in",
            AuditSeverity.Info
        );

        // Act — POST
        var postResponse = await _client.PostAsJsonAsync("/api/audit-logs", request);

        // Assert POST
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        // Act — GET
        var getResponse = await _client.GetAsync("/api/audit-logs?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var logs = await getResponse.Content.ReadFromJsonAsync<List<AuditLogResponse>>();

        Assert.NotNull(logs);
        Assert.NotEmpty(logs);

        var log = logs!.First();

        // Assert fields
        Assert.Equal("identity-service", log.ServiceName);
        Assert.Equal("UserLoggedIn", log.ActionName);
        Assert.Equal("Successfully logged in", log.Message);
        Assert.Equal(AuditSeverity.Info, log.Severity);
        Assert.True(log.Timestamp > DateTimeOffset.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task CreateMultipleLogs_ShouldReturnAll()
    {
        for (var i = 0; i < 3; i++)
        {
            var req = new AuditLogRequest(
                "test-service",
                $"Action{i}",
                "Log message",
                AuditSeverity.Warning
            );

            await _client.PostAsJsonAsync("/api/audit-logs", req);
        }

        var response = await _client.GetAsync("/api/audit-logs?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var logs = await response.Content.ReadFromJsonAsync<List<AuditLogResponse>>();

        Assert.NotNull(logs);
        Assert.True(logs!.Count >= 3);
    }

    [Fact]
    public async Task CreateAuditLog_ShouldReturn201()
    {
        var req = new AuditLogRequest(
            "billing-service",
            "InvoiceCreated",
            "Invoice #100 created",
            AuditSeverity.Info
        );

        var res = await _client.PostAsJsonAsync("/api/audit-logs", req);

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }
}