namespace SachkovTech.Accounts.Contracts.Dtos;

public record SocialNetworkDto
{
    public string Name { get; init; }
    
    public string Link { get; init; }
}