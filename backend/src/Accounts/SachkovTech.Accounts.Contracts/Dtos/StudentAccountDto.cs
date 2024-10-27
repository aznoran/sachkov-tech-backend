using SachkovTech.Core.Dtos;

namespace SachkovTech.Accounts.Contracts.Dtos;

public class StudentAccountDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public IEnumerable<SocialNetworkDto>? SocialNetworks { get; init; } = [];

    public DateTime DateStartedStudying { get; init; }
}