namespace VoterRegistryService.Application.Contracts.Requests;

public sealed record CreateVoterRequest(
    string NationalId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Country,
    string? Address,
    bool IsResident);