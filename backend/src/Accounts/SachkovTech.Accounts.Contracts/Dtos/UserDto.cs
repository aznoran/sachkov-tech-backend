namespace SachkovTech.Accounts.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = default!;

    public string SecondName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public StudentAccountDto? StudentAccount { get; init; }

    public SupportAccountDto? SupportAccount { get; init; }

    public AdminAccountDto? AdminAccount { get; init; }

    public List<UserRolesDto> UserRoles { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];

    public IReadOnlyList<SocialNetworkDto> SocialNetworks { get; init; } = [];
}