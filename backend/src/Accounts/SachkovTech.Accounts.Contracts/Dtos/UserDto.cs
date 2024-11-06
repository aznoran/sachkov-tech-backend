using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Accounts.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = default!;

    public string SecondName { get; init; } = default!;

    public StudentAccountDto StudentAccount { get; init; } = default!;

    public SupportAccountDto SupportAccount { get; init; } = default!;
    
    public AdminAccountDto AdminAccount { get; init; } = default!;
    
    public List<UserRolesDto> UserRoles { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];
    
    public IReadOnlyList<SocialNetworkDto> SocialNetworks { get; init; } = [];
}