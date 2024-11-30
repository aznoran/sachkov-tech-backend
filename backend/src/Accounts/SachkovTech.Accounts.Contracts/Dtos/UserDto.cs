namespace SachkovTech.Accounts.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string SecondName { get; init; } = string.Empty;
    public string ThirdName { get; init; } = string.Empty;

    public DateTime RegistrationDate { get; init; }
    public string Email { get; init; } = string.Empty;

    public StudentAccountDto? StudentAccount { get; init; }

    public SupportAccountDto? SupportAccount { get; init; }

    public AdminAccountDto? AdminAccount { get; init; }

    public List<UserRolesDto> UserRoles { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];

    public IReadOnlyList<SocialNetworkDto> SocialNetworks { get; init; } = [];
}