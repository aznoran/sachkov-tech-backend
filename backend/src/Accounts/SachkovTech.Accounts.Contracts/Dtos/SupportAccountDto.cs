using SachkovTech.Core.Dtos;

namespace SachkovTech.Accounts.Contracts.Dtos;

public class SupportAccountDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public IEnumerable<SocialNetworkDto> SocialNetworks { get; init; } = [];

    public string AboutSelf { get; init; } = default!;
}