namespace SachkovTech.Accounts.Contracts.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = default!;

    public string SecondName { get; init; } = default!;

    public StudentAccountDto StudentAccount { get; init; } = default!;

    public SupportAccountDto SupportAccount { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];
}