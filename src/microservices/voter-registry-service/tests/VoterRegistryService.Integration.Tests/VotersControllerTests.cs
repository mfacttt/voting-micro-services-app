using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoterRegistryService.Application.Contracts.Requests;
using VoterRegistryService.Application.Contracts.Responses;
using VoterRegistryService.Domain.Enums;
using VoterRegistryService.Persistence.Context;

namespace VoterRegistryService.Integration.Tests;

public class VotersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VotersControllerTests(WebApplicationFactory<Program> factory)
    {
        var configuredFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<VoterRegistryDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<VoterRegistryDbContext>(options => { options.UseInMemoryDatabase("VoterRegistryTestDb"); });
            });
        });

        _client = configuredFactory.CreateClient();
    }

    [Fact]
    public async Task CreateVoter_ReturnsCreated()
    {
        var request = new CreateVoterRequest(
            "NAT-123",
            "John",
            "Doe",
            new DateOnly(1990, 1, 1),
            "MD",
            "Chisinau",
            true
        );

        var response = await _client.PostAsJsonAsync("api/v1/voters", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var voter = await response.Content.ReadFromJsonAsync<VoterResponse>();
        Assert.NotNull(voter);
        Assert.Equal("NAT-123", voter.NationalId);
    }

    [Fact]
    public async Task GetById_ReturnsOk()
    {
        var request = new CreateVoterRequest(
            "NAT-222",
            "Alice",
            "Smith",
            new DateOnly(1985, 2, 2),
            "MD",
            "Chisinau",
            true);

        var create = await _client.PostAsJsonAsync("api/v1/voters", request);
        var voter = await create.Content.ReadFromJsonAsync<VoterResponse>();

        var response = await _client.GetAsync($"api/v1/voters/{voter!.Id}");
        var result = await response.Content.ReadFromJsonAsync<VoterResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Alice", result!.FirstName);
    }

    [Fact]
    public async Task GetByNationalId_ReturnsOk()
    {
        var request = new CreateVoterRequest(
            "NAT-333",
            "Mike",
            "Ross",
            new DateOnly(1991, 5, 1),
            "MD",
            null,
            true);

        await _client.PostAsJsonAsync("api/v1/voters", request);

        var response = await _client.GetAsync("api/v1/voters/national/NAT-333");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var voter = await response.Content.ReadFromJsonAsync<VoterResponse>();

        Assert.NotNull(voter);
        Assert.Equal("Mike", voter.FirstName);
    }

    [Fact]
    public async Task UpdateStatus_ReturnsOk()
    {
        var request = new CreateVoterRequest(
            "NAT-444",
            "Bob",
            "Taylor",
            new DateOnly(1980, 7, 7),
            "MD",
            null,
            true);

        var create = await _client.PostAsJsonAsync("api/v1/voters", request);
        var voter = await create.Content.ReadFromJsonAsync<VoterResponse>();

        var update = new UpdateVoterStatusRequest(voter!.Id, VoterStatus.Inactive);

        var response = await _client.PatchAsJsonAsync($"api/v1/voters/{voter.Id}/status", update);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content.ReadFromJsonAsync<VoterResponse>();

        Assert.NotNull(updated);
        Assert.Equal(VoterStatus.Inactive, updated.Status);
    }

    [Fact]
    public async Task GetPaged_ReturnsList()
    {
        for (var i = 0; i < 3; i++)
        {
            var req = new CreateVoterRequest(
                $"NAT-{100 + i}",
                $"User{i}",
                "Test",
                new DateOnly(1990, 1, 1),
                "MD",
                null,
                true);

            await _client.PostAsJsonAsync("api/v1/voters", req);
        }

        var response = await _client.GetAsync("api/v1/voters?page=1&pageSize=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<List<VoterResponse>>();

        Assert.NotNull(list);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetById_NotFound()
    {
        var response = await _client.GetAsync($"api/v1/voters/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}