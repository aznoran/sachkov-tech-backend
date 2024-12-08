namespace SachkovTech.Accounts.Contracts.Responses;

public record LoginResponse(
    string AccessToken,
    Guid RefreshToken,
    Guid UserId,
    IEnumerable<string> Roles);